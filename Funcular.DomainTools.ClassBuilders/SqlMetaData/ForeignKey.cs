#region File info

// *********************************************************************************************************
// DomainTools>DomainTools.ClassBuilders>SchemaColumnInfo.cs
// Created: 2014-10-06 5:36 PM
// Updated: 2015-06-04 2:01 PM
// By: Paul Smith 
// *********************************************************************************************************

#endregion



#region Usings



#endregion



namespace Funcular.DomainTools.ClassBuilders.SqlMetaData
{
    public partial class ForeignKey
    {
       public string RelationshipName  {get;set;}
       public string FromSchema        {get;set;}
       public string FromTable         {get;set;}
       public string FromColumn        {get;set;}
       public string ToSchema          {get;set;}
       public string ToTable           {get;set;}
       public string ToColumn          {get;set;}
    }
}