using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AppBase.ORM
{
    public class Model
    {
        /// <summary>
        /// Get or set entities
        /// </summary>
        [JsonProperty("entities")]
        public IList<ModelEntity> Entities { get; set; }

        /// <summary>
        /// Get entity type
        /// </summary>
        /// <param name="relation">Relation name</param>
        /// <param name="end1">Entity relation END 1</param>
        /// <returns>Type</returns>
        public string GetEntityType(string relation, ModelEntity end1)
        {
            var chain = GetRelationEntityChain(
                relation,
                end1
                );
            return
                (chain.RelationType == ModelRelationEntityChainType.ManyToMany ||
                 chain.RelationType == ModelRelationEntityChainType.OneToMany)
                    ? "BaseEntityCollection<" + chain.End2.Name + ">"
                    : chain.End2.Name;
        }

        /// <summary>
        /// Get relation entity chain
        /// </summary>
        /// <param name="relation">Relation name</param>
        /// <param name="end1">Entity relation END 1</param>
        /// <returns>Entity chain</returns>
        public ModelRelationEntityChain GetRelationEntityChain(string relation, ModelEntity end1)
        {
            var chain = new ModelRelationEntityChain();
            chain.End1 = end1;

            #region Seek chain
            var path = relation
                .Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (path.Length != 2)
                throw new ModelException("Malformed relation");
            chain.Parent = Entities.FirstOrDefault(
                x => x.Name.Equals(path[0], StringComparison.InvariantCultureIgnoreCase));
            if (chain.Parent == null)
                throw new ModelException("Unknown entity \"" + path[0] + "\"");
            chain.Relation = chain.Parent.Relations.FirstOrDefault(
                x => x.Name.Equals(path[1], StringComparison.InvariantCultureIgnoreCase));
            if (chain.Relation == null)
                throw new ModelException("Relation \"" + path[1] + "\" cannot be found on \"" + path[0] + "\"");
            chain.End2 = Entities.FirstOrDefault(
                x => x.Name.Equals(chain.Relation.EntityName, StringComparison.InvariantCultureIgnoreCase));
            if (chain.End2 == null)
                throw new ModelException("Referenced entity \"" + chain.Relation.EntityName +
                    "\" cannot be found by \"" + chain.Relation.Name + "\"");

            // @if END 1 is the same as END 2 then relation parent should become the new END 2;
            if (chain.End1 == chain.End2)
                chain.End2 =
                    chain.Parent;
            #endregion

            #region Find relation type
            if (chain.End1 != chain.Parent &&
                chain.End1 != chain.End2 && chain.Parent != chain.End2)
            {
                chain.RelationType = ModelRelationEntityChainType.ManyToMany;
                if (!chain.Parent.Fields.Where(
                    x => string.IsNullOrEmpty(x.Relation)).All(x => x.IsKey == true))
                {
                    chain.End2 = chain.Parent;
                    chain.RelationType = ModelRelationEntityChainType.ManyToOne;
                }
            }
            else
            {
                var relationEnd1KeyFields = new Dictionary<string, ModelField>(StringComparer.InvariantCultureIgnoreCase);
                foreach (var item in chain.Relation.Fields)
                    relationEnd1KeyFields.Add(item.ParentColumnName,
                        chain.End1.Fields.FirstOrDefault(x =>
                            x.ColumnName != null &&
                            x.ColumnName.Equals(item.ParentColumnName, StringComparison.InvariantCultureIgnoreCase))
                            );
                var relationEnd2KeyFields = new Dictionary<string, ModelField>(StringComparer.InvariantCultureIgnoreCase);
                foreach (var item in chain.Relation.Fields)
                    relationEnd2KeyFields.Add(item.ChildColumnName,
                        chain.End2.Fields.FirstOrDefault(x =>
                            x.ColumnName != null &&
                            x.ColumnName.Equals(item.ChildColumnName, StringComparison.InvariantCultureIgnoreCase))
                            );

                var numOfKeysOnEnd1 = chain.End1.Fields.Count(x => x.IsKey == true);
                var numOfKeysOnEnd2 = chain.End2.Fields.Count(x => x.IsKey == true);
                if (numOfKeysOnEnd1 == numOfKeysOnEnd2 &&
                    relationEnd1KeyFields.Count == relationEnd2KeyFields.Count &&
                    numOfKeysOnEnd1 == relationEnd1KeyFields.Count)
                    chain.RelationType = ModelRelationEntityChainType.OneToOne;
                else if (numOfKeysOnEnd1 == relationEnd1KeyFields.Count)
                    chain.RelationType = ModelRelationEntityChainType.OneToMany;
                else if (numOfKeysOnEnd2 == relationEnd2KeyFields.Count)
                    chain.RelationType = ModelRelationEntityChainType.ManyToOne;
            }
            #endregion

            return chain;
        }

        /// <summary>
        /// Get entity relation hierarchy
        /// </summary>
        /// <param name="rootEntity">Root entity</param>
        /// <returns>Entity hierarchy</returns>
        public List<ModelEntity> GetEntityRelationHierarchy(ModelEntity rootEntity)
        {
            var bag = new Dictionary<ModelEntity, HashSet<ModelEntity>>();
            var res = new List<ModelEntity>();

            #region Collect dependencies
            Action<ModelEntity> depCollAct = null;
            depCollAct =
                new Action<ModelEntity>((currEntity) =>
                {
                    if (bag.ContainsKey(currEntity))
                        return;
                    bag.Add(currEntity, new HashSet<ModelEntity>());
                    foreach (var field in
                        currEntity.Fields.Where(x => !string.IsNullOrEmpty(x.Relation)))
                    {
                        var chain = GetRelationEntityChain(
                            field.Relation,
                            currEntity
                            );
                        switch (chain.RelationType)
                        {
                            case ModelRelationEntityChainType.ManyToMany:
                                depCollAct(chain.Parent);
                                break;
                            case ModelRelationEntityChainType.ManyToOne:
                                bag[currEntity].Add(chain.End2);
                                depCollAct(chain.End2);
                                break;
                            default:
                                depCollAct(chain.End2);
                                break;
                        }
                    }
                });
            depCollAct(rootEntity);
            #endregion

            #region Sort by dependencies
            while (bag.Count > 0)
            {
                var currEntity =
                    bag.FirstOrDefault(x => x.Value.Count == 0);
                if (currEntity.Key == null)
                    currEntity = bag.First();

                bag.Remove(currEntity.Key);

                foreach (var entity in bag)
                    entity.Value.Remove(currEntity.Key);

                res.Add(currEntity.Key);
            }
            #endregion

            return res;
        }

        /// <summary>
        /// Serialize object
        /// </summary>
        /// <returns>A JSON</returns>
        public string SerializeObject()
        {
            return JsonConvert.SerializeObject(this,
                Formatting.Indented,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }
                );
        }

        /// <summary>
        /// Deserialize object
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>Model</returns>
        public static Model DeserializeObject(string json)
        {
            return JsonConvert.DeserializeObject<Model>(json);
        }

        /// <summary>
        /// Save JSON model to file
        /// </summary>
        /// <param name="path">Path</param>
        public void Save(string path)
        {
            using (var file = new StreamWriter(path, false, new UTF8Encoding(false)))
                file.Write(SerializeObject());
        }

        /// <summary>
        /// Load model from a JSON file
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>Model</returns>
        public static Model Load(string path)
        {
            using (var file = new StreamReader(path, new UTF8Encoding(false)))
                return DeserializeObject(file.ReadToEnd());
        }
    }
}
