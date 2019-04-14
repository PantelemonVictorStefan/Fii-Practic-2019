namespace AppBase.ORM
{
    public enum ModelRelationEntityChainType
    {
        /// <summary>
        /// Unknown relationship type
        /// </summary>
        Unknown,
        /// <summary>
        /// One-One Relationship (1-1 Relationship)
        /// </summary>
        OneToOne,
        /// <summary>
        /// One-Many Relationship (1-M Relationship)
        /// </summary>
        OneToMany,
        /// <summary>
        /// Many-One Relationship (M-1 Relationship)
        /// </summary>
        ManyToOne,
        /// <summary>
        /// Many-Many Relationship (M-M Relationship)
        /// </summary>
        ManyToMany
    }
}
