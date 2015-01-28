﻿// Type: FluentNHibernate.Mapping.ManyToManyPart`1
// Assembly: FluentNHibernate, Version=2.0.1.0, Culture=neutral
// MVID: 24E93491-0053-4676-B166-4BA6A0EA99AE
// Assembly location: D:\Mis ejemplos\Nhibernate\FluentNh\packages\FluentNHibernate.2.0.1.0\lib\net40\FluentNHibernate.dll

using FluentNHibernate;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentNHibernate.Mapping
{
  public class ManyToManyPart<TChild> : ToManyBase<ManyToManyPart<TChild>, TChild>
  {
    private readonly IList<IFilterMappingProvider> childFilters = (IList<IFilterMappingProvider>) new List<IFilterMappingProvider>();
    private readonly FetchTypeExpression<ManyToManyPart<TChild>> fetch;
    private readonly NotFoundExpression<ManyToManyPart<TChild>> notFound;
    private IndexManyToManyPart manyToManyIndex;
    private IndexPart index;
    private readonly ColumnMappingCollection<ManyToManyPart<TChild>> childKeyColumns;
    private readonly ColumnMappingCollection<ManyToManyPart<TChild>> parentKeyColumns;
    private readonly Type childType;
    private Type valueType;
    private bool isTernary;

    public ColumnMappingCollection<ManyToManyPart<TChild>> ChildKeyColumns
    {
      get
      {
        return this.childKeyColumns;
      }
    }

    public ColumnMappingCollection<ManyToManyPart<TChild>> ParentKeyColumns
    {
      get
      {
        return this.parentKeyColumns;
      }
    }

    public FetchTypeExpression<ManyToManyPart<TChild>> FetchType
    {
      get
      {
        return this.fetch;
      }
    }

    public Type ChildType
    {
      get
      {
        return typeof (TChild);
      }
    }

    public NotFoundExpression<ManyToManyPart<TChild>> NotFound
    {
      get
      {
        return this.notFound;
      }
    }

    public ManyToManyPart(Type entity, Member property)
      : this(entity, property, property.PropertyType)
    {
    }

    protected ManyToManyPart(Type entity, Member member, Type collectionType)
      : base(entity, member, collectionType)
    {
      this.childType = collectionType;
      this.fetch = new FetchTypeExpression<ManyToManyPart<TChild>>(this, (Action<string>) (value => this.collectionAttributes.Set("Fetch", 2, (object) value)));
      this.notFound = new NotFoundExpression<ManyToManyPart<TChild>>(this, (Action<string>) (value => this.relationshipAttributes.Set("NotFound", 2, (object) value)));
      this.childKeyColumns = new ColumnMappingCollection<ManyToManyPart<TChild>>(this);
      this.parentKeyColumns = new ColumnMappingCollection<ManyToManyPart<TChild>>(this);
    }

    /// <summary>
    /// Sets a single child key column. If there are multiple columns, use ChildKeyColumns.Add
    /// 
    /// </summary>
    public ManyToManyPart<TChild> ChildKeyColumn(string childKeyColumn)
    {
      this.childKeyColumns.Clear();
      this.childKeyColumns.Add(childKeyColumn);
      return this;
    }

    /// <summary>
    /// Sets a single parent key column. If there are multiple columns, use ParentKeyColumns.Add
    /// 
    /// </summary>
    public ManyToManyPart<TChild> ParentKeyColumn(string parentKeyColumn)
    {
      this.parentKeyColumns.Clear();
      this.parentKeyColumns.Add(parentKeyColumn);
      return this;
    }

    public ManyToManyPart<TChild> ForeignKeyConstraintNames(string parentForeignKeyName, string childForeignKeyName)
    {
      this.keyMapping.Set<string>((Expression<Func<KeyMapping, string>>) (x => x.ForeignKey), 2, parentForeignKeyName);
      this.relationshipAttributes.Set("ForeignKey", 2, (object) childForeignKeyName);
      return this;
    }

    public ManyToManyPart<TChild> ChildPropertyRef(string childPropertyRef)
    {
      this.relationshipAttributes.Set("ChildPropertyRef", 2, (object) childPropertyRef);
      return this;
    }

    private void EnsureDictionary()
    {
      if (!typeof (IDictionary).IsAssignableFrom(this.childType))
        throw new ArgumentException(this.member.Name + (object) " must be of type IDictionary to be used in a non-generic ternary association. Type was: " + (string) (object) this.childType);
    }

    private void EnsureGenericDictionary()
    {
      if (!this.childType.IsGenericType || !(this.childType.GetGenericTypeDefinition() == typeof (IDictionary<,>)))
        throw new ArgumentException(this.member.Name + (object) " must be of type IDictionary<> to be used in a ternary assocation. Type was: " + (string) (object) this.childType);
    }

    public ManyToManyPart<TChild> AsTernaryAssociation()
    {
      this.EnsureGenericDictionary();
      return this.AsTernaryAssociation(typeof (TChild).GetGenericArguments()[0].Name + "_id", typeof (TChild).GetGenericArguments()[1].Name + "_id");
    }

    public ManyToManyPart<TChild> AsTernaryAssociation(string indexColumn, string valueColumn)
    {
      return this.AsTernaryAssociation(indexColumn, valueColumn, (Action<IndexManyToManyPart>) (x => {}));
    }

    public ManyToManyPart<TChild> AsTernaryAssociation(string indexColumn, string valueColumn, Action<IndexManyToManyPart> indexAction)
    {
      this.EnsureGenericDictionary();
      Type indexType = typeof (TChild).GetGenericArguments()[0];
      Type type = typeof (TChild).GetGenericArguments()[1];
      this.manyToManyIndex = new IndexManyToManyPart(typeof (ManyToManyPart<TChild>));
      this.manyToManyIndex.Column(indexColumn);
      this.manyToManyIndex.Type(indexType);
      if (indexAction != null)
        indexAction(this.manyToManyIndex);
      this.ChildKeyColumn(valueColumn);
      this.valueType = type;
      this.isTernary = true;
      return this;
    }

    public ManyToManyPart<TChild> AsTernaryAssociation(Type indexType, Type typeOfValue)
    {
      return this.AsTernaryAssociation(indexType, indexType.Name + "_id", typeOfValue, typeOfValue.Name + "_id");
    }

    public ManyToManyPart<TChild> AsTernaryAssociation(Type indexType, string indexColumn, Type typeOfValue, string valueColumn)
    {
      return this.AsTernaryAssociation(indexType, indexColumn, typeOfValue, valueColumn, (Action<IndexManyToManyPart>) (x => {}));
    }

    public ManyToManyPart<TChild> AsTernaryAssociation(Type indexType, string indexColumn, Type typeOfValue, string valueColumn, Action<IndexManyToManyPart> indexAction)
    {
      this.EnsureDictionary();
      this.manyToManyIndex = new IndexManyToManyPart(typeof (ManyToManyPart<TChild>));
      this.manyToManyIndex.Column(indexColumn);
      this.manyToManyIndex.Type(indexType);
      if (indexAction != null)
        indexAction(this.manyToManyIndex);
      this.ChildKeyColumn(valueColumn);
      this.valueType = typeOfValue;
      this.isTernary = true;
      return this;
    }

    public ManyToManyPart<TChild> AsSimpleAssociation()
    {
      this.EnsureGenericDictionary();
      return this.AsSimpleAssociation(typeof (TChild).GetGenericArguments()[0].Name + "_id", typeof (TChild).GetGenericArguments()[1].Name + "_id");
    }

    public ManyToManyPart<TChild> AsSimpleAssociation(string indexColumn, string valueColumn)
    {
      this.EnsureGenericDictionary();
      Type type1 = typeof (TChild).GetGenericArguments()[0];
      Type type2 = typeof (TChild).GetGenericArguments()[1];
      this.index = new IndexPart(type1);
      this.index.Column(indexColumn);
      this.index.Type(type1);
      this.ChildKeyColumn(valueColumn);
      this.valueType = type2;
      this.isTernary = true;
      return this;
    }

    public ManyToManyPart<TChild> AsEntityMap()
    {
      return this.AsMap((string) null).AsTernaryAssociation();
    }

    public ManyToManyPart<TChild> AsEntityMap(string indexColumn, string valueColumn)
    {
      return this.AsMap((string) null).AsTernaryAssociation(indexColumn, valueColumn);
    }

    protected override ICollectionRelationshipMapping GetRelationship()
    {
      ManyToManyMapping manyToManyMapping = new ManyToManyMapping(this.relationshipAttributes.Clone())
      {
        ContainingEntityType = this.EntityType
      };
      if (this.isTernary && this.valueType != (Type) null)
        manyToManyMapping.Set<TypeReference>((Expression<Func<ManyToManyMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(this.valueType));
      foreach (IFilterMappingProvider filterMappingProvider in (IEnumerable<IFilterMappingProvider>) this.childFilters)
        manyToManyMapping.ChildFilters.Add(filterMappingProvider.GetFilterMapping());
      return (ICollectionRelationshipMapping) manyToManyMapping;
    }

    /// <summary>
    /// Sets the order-by clause on the collection element.
    /// 
    /// </summary>
    public ManyToManyPart<TChild> OrderBy(string orderBy)
    {
      this.collectionAttributes.Set("OrderBy", 2, (object) orderBy);
      return this;
    }

    /// <summary>
    /// Sets the order-by clause on the many-to-many element.
    /// 
    /// </summary>
    public ManyToManyPart<TChild> ChildOrderBy(string orderBy)
    {
      this.relationshipAttributes.Set("OrderBy", 2, (object) orderBy);
      return this;
    }

    public ManyToManyPart<TChild> ReadOnly()
    {
      this.collectionAttributes.Set("Mutable", 2, (object) (bool) (!this.nextBool ? 1 : 0));
      this.nextBool = true;
      return this;
    }

    public ManyToManyPart<TChild> Subselect(string subselect)
    {
      this.collectionAttributes.Set("Subselect", 2, (object) subselect);
      return this;
    }

    /// <overloads>Applies a filter to the child element of this entity given it's name.
    ///             </overloads>
    /// <summary>
    /// Applies a filter to the child element of this entity given it's name.
    /// 
    /// </summary>
    /// <param name="name">The filter's name</param><param name="condition">The condition to apply</param>
    public ManyToManyPart<TChild> ApplyChildFilter(string name, string condition)
    {
      this.childFilters.Add((IFilterMappingProvider) new FilterPart(name, condition));
      return this;
    }

    /// <overloads>Applies a filter to the child element of this entity given it's name.
    ///             </overloads>
    /// <summary>
    /// Applies a filter to the child element of this entity given it's name.
    /// 
    /// </summary>
    /// <param name="name">The filter's name</param>
    public ManyToManyPart<TChild> ApplyChildFilter(string name)
    {
      return this.ApplyChildFilter(name, (string) null);
    }

    /// <overloads>Applies a named filter to the child element of this many-to-many.
    ///             </overloads>
    /// <summary>
    /// Applies a named filter to the child element of this many-to-many.
    /// 
    /// </summary>
    /// <param name="condition">The condition to apply</param><typeparam name="TFilter">The type of a <see cref="T:FluentNHibernate.Mapping.FilterDefinition"/> implementation
    ///             defining the filter to apply.
    ///             </typeparam>
    public ManyToManyPart<TChild> ApplyChildFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
    {
      this.childFilters.Add((IFilterMappingProvider) new FilterPart(Activator.CreateInstance<TFilter>().Name, condition));
      return this;
    }

    /// <summary>
    /// Applies a named filter to the child element of this many-to-many.
    /// 
    /// </summary>
    /// <typeparam name="TFilter">The type of a <see cref="T:FluentNHibernate.Mapping.FilterDefinition"/> implementation
    ///             defining the filter to apply.
    ///             </typeparam>
    public ManyToManyPart<TChild> ApplyChildFilter<TFilter>() where TFilter : FilterDefinition, new()
    {
      return this.ApplyChildFilter<TFilter>((string) null);
    }

    /// <summary>
    /// Sets the where clause for this relationship, on the many-to-many element.
    /// 
    /// </summary>
    public ManyToManyPart<TChild> ChildWhere(string where)
    {
      this.relationshipAttributes.Set("Where", 2, (object) where);
      return this;
    }

    /// <summary>
    /// Sets the where clause for this relationship, on the many-to-many element.
    ///             Note: This only supports simple cases, use the string overload for more complex clauses.
    /// 
    /// </summary>
    public ManyToManyPart<TChild> ChildWhere(Expression<Func<TChild, bool>> where)
    {
      return this.ChildWhere(ExpressionToSql.Convert<TChild>(where));
    }

    protected override CollectionMapping GetCollectionMapping()
    {
      CollectionMapping collectionMapping = base.GetCollectionMapping();
      if (this.parentKeyColumns.Count == 0)
        collectionMapping.Key.AddColumn(0, new ColumnMapping(this.EntityType.Name + "_id"));
      foreach (ColumnMapping mapping in this.parentKeyColumns)
        collectionMapping.Key.AddColumn(2, mapping);
      if (collectionMapping.Relationship != null)
      {
        if (this.childKeyColumns.Count == 0)
          ((ManyToManyMapping) collectionMapping.Relationship).AddColumn(0, new ColumnMapping(typeof (TChild).Name + "_id"));
        foreach (ColumnMapping mapping in this.childKeyColumns)
          ((ManyToManyMapping) collectionMapping.Relationship).AddColumn(2, mapping);
      }
      if (this.index != null)
        collectionMapping.Set<IIndexMapping>((Expression<Func<CollectionMapping, IIndexMapping>>) (x => x.Index), 0, (IIndexMapping) this.index.GetIndexMapping());
      if (this.manyToManyIndex != null && collectionMapping.Collection == Collection.Map)
        collectionMapping.Set<IIndexMapping>((Expression<Func<CollectionMapping, IIndexMapping>>) (x => x.Index), 0, (IIndexMapping) this.manyToManyIndex.GetIndexMapping());
      return collectionMapping;
    }
  }
}
