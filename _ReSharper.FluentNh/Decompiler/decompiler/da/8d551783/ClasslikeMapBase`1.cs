// Type: FluentNHibernate.Mapping.ClasslikeMapBase`1
// Assembly: FluentNHibernate, Version=2.0.1.0, Culture=neutral
// MVID: 24E93491-0053-4676-B166-4BA6A0EA99AE
// Assembly location: D:\Mis ejemplos\Nhibernate\FluentNh\packages\FluentNHibernate.2.0.1.0\lib\net40\FluentNHibernate.dll

using FluentNHibernate;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentNHibernate.Mapping
{
  public abstract class ClasslikeMapBase<T>
  {
    private readonly MappingProviderStore providers;

    internal IEnumerable<IPropertyMappingProvider> Properties
    {
      get
      {
        return (IEnumerable<IPropertyMappingProvider>) this.providers.Properties;
      }
    }

    internal IEnumerable<IComponentMappingProvider> Components
    {
      get
      {
        return (IEnumerable<IComponentMappingProvider>) this.providers.Components;
      }
    }

    internal Type EntityType
    {
      get
      {
        return typeof (T);
      }
    }

    protected ClasslikeMapBase(MappingProviderStore providers)
    {
      this.providers = providers;
    }

    /// <summary>
    /// Called when a member is mapped by a builder method.
    /// 
    /// </summary>
    /// <param name="member">Member being mapped.</param>
    internal virtual void OnMemberMapped(Member member)
    {
    }

    /// <summary>
    /// Create a property mapping.
    /// 
    /// </summary>
    /// <param name="memberExpression">Property to map</param>
    /// <example>
    /// Map(x =&gt; x.Name);
    /// 
    /// </example>
    public PropertyPart Map(Expression<Func<T, object>> memberExpression)
    {
      return this.Map(memberExpression, (string) null);
    }

    /// <summary>
    /// Create a property mapping.
    /// 
    /// </summary>
    /// <param name="memberExpression">Property to map</param><param name="columnName">Property column name</param>
    /// <example>
    /// Map(x =&gt; x.Name, "person_name");
    /// 
    /// </example>
    public PropertyPart Map(Expression<Func<T, object>> memberExpression, string columnName)
    {
      return this.Map(ReflectionExtensions.ToMember<T, object>(memberExpression), columnName);
    }

    private PropertyPart Map(Member member, string columnName)
    {
      this.OnMemberMapped(member);
      PropertyPart propertyPart = new PropertyPart(member, typeof (T));
      if (!string.IsNullOrEmpty(columnName))
        propertyPart.Column(columnName);
      this.providers.Properties.Add((IPropertyMappingProvider) propertyPart);
      return propertyPart;
    }

    /// <summary>
    /// Create a reference to another entity. In database terms, this is a many-to-one
    ///             relationship.
    /// 
    /// </summary>
    /// <typeparam name="TOther">Other entity</typeparam><param name="memberExpression">Property on the current entity</param>
    /// <example>
    /// References(x =&gt; x.Company);
    /// 
    /// </example>
    public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> memberExpression)
    {
      return this.References<TOther>(memberExpression, (string) null);
    }

    /// <summary>
    /// Create a reference to another entity. In database terms, this is a many-to-one
    ///             relationship.
    /// 
    /// </summary>
    /// <typeparam name="TOther">Other entity</typeparam><param name="memberExpression">Property on the current entity</param><param name="columnName">Column name</param>
    /// <example>
    /// References(x =&gt; x.Company, "company_id");
    /// 
    /// </example>
    public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, TOther>> memberExpression, string columnName)
    {
      return this.References<TOther>(ReflectionExtensions.ToMember<T, TOther>(memberExpression), columnName);
    }

    /// <summary>
    /// Create a reference to another entity. In database terms, this is a many-to-one
    ///             relationship.
    /// 
    /// </summary>
    /// <typeparam name="TOther">Other entity</typeparam><param name="memberExpression">Property on the current entity</param>
    /// <example>
    /// References(x =&gt; x.Company, "company_id");
    /// 
    /// </example>
    public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, object>> memberExpression)
    {
      return this.References<TOther>(memberExpression, (string) null);
    }

    /// <summary>
    /// Create a reference to another entity. In database terms, this is a many-to-one
    ///             relationship.
    /// 
    /// </summary>
    /// <typeparam name="TOther">Other entity</typeparam><param name="memberExpression">Property on the current entity</param><param name="columnName">Column name</param>
    /// <example>
    /// References(x =&gt; x.Company, "company_id");
    /// 
    /// </example>
    public ManyToOnePart<TOther> References<TOther>(Expression<Func<T, object>> memberExpression, string columnName)
    {
      return this.References<TOther>(ReflectionExtensions.ToMember<T, object>(memberExpression), columnName);
    }

    private ManyToOnePart<TOther> References<TOther>(Member member, string columnName)
    {
      this.OnMemberMapped(member);
      ManyToOnePart<TOther> manyToOnePart = new ManyToOnePart<TOther>(this.EntityType, member);
      if (columnName != null)
        manyToOnePart.Column(columnName);
      this.providers.References.Add((IManyToOneMappingProvider) manyToOnePart);
      return manyToOnePart;
    }

    /// <summary>
    /// Create a reference to any other entity. This is an "any" polymorphic relationship.
    /// 
    /// </summary>
    /// <typeparam name="TOther">Other entity to reference</typeparam><param name="memberExpression">Property</param>
    public AnyPart<TOther> ReferencesAny<TOther>(Expression<Func<T, TOther>> memberExpression)
    {
      return this.ReferencesAny<TOther>(ReflectionExtensions.ToMember<T, TOther>(memberExpression));
    }

    private AnyPart<TOther> ReferencesAny<TOther>(Member member)
    {
      this.OnMemberMapped(member);
      AnyPart<TOther> anyPart = new AnyPart<TOther>(typeof (T), member);
      this.providers.Anys.Add((IAnyMappingProvider) anyPart);
      return anyPart;
    }

    /// <summary>
    /// Create a reference to another entity based exclusively on the primary-key values.
    ///             This is sometimes called a one-to-one relationship, in database terms. Generally
    ///             you should use <see cref="M:FluentNHibernate.Mapping.ClasslikeMapBase`1.References``1(System.Linq.Expressions.Expression{System.Func{`0,System.Object}})"/>
    ///             whenever possible.
    /// 
    /// </summary>
    /// <typeparam name="TOther">Other entity</typeparam><param name="memberExpression">Property</param>
    /// <example>
    /// HasOne(x =&gt; x.ExtendedInfo);
    /// 
    /// </example>
    public OneToOnePart<TOther> HasOne<TOther>(Expression<Func<T, object>> memberExpression)
    {
      return this.HasOne<TOther>(ReflectionExtensions.ToMember<T, object>(memberExpression));
    }

    /// <summary>
    /// Create a reference to another entity based exclusively on the primary-key values.
    ///             This is sometimes called a one-to-one relationship, in database terms. Generally
    ///             you should use <see cref="M:FluentNHibernate.Mapping.ClasslikeMapBase`1.References``1(System.Linq.Expressions.Expression{System.Func{`0,System.Object}})"/>
    ///             whenever possible.
    /// 
    /// </summary>
    /// <typeparam name="TOther">Other entity</typeparam><param name="memberExpression">Property</param>
    /// <example>
    /// HasOne(x =&gt; x.ExtendedInfo);
    /// 
    /// </example>
    public OneToOnePart<TOther> HasOne<TOther>(Expression<Func<T, TOther>> memberExpression)
    {
      return this.HasOne<TOther>(ReflectionExtensions.ToMember<T, TOther>(memberExpression));
    }

    private OneToOnePart<TOther> HasOne<TOther>(Member member)
    {
      this.OnMemberMapped(member);
      OneToOnePart<TOther> oneToOnePart = new OneToOnePart<TOther>(this.EntityType, member);
      this.providers.OneToOnes.Add((IOneToOneMappingProvider) oneToOnePart);
      return oneToOnePart;
    }

    /// <summary>
    /// Create a dynamic component mapping. This is a dictionary that represents
    ///             a limited number of columns in the database.
    /// 
    /// </summary>
    /// <param name="memberExpression">Property containing component</param><param name="dynamicComponentAction">Component setup action</param>
    /// <example>
    /// DynamicComponent(x =&gt; x.Data, comp =&gt;
    ///             {
    ///               comp.Map(x =&gt; (int)x["age"]);
    ///             });
    /// 
    /// </example>
    public DynamicComponentPart<IDictionary> DynamicComponent(Expression<Func<T, IDictionary>> memberExpression, Action<DynamicComponentPart<IDictionary>> dynamicComponentAction)
    {
      return this.DynamicComponent(ReflectionExtensions.ToMember<T, IDictionary>(memberExpression), dynamicComponentAction);
    }

    private DynamicComponentPart<IDictionary> DynamicComponent(Member member, Action<DynamicComponentPart<IDictionary>> dynamicComponentAction)
    {
      this.OnMemberMapped(member);
      DynamicComponentPart<IDictionary> dynamicComponentPart = new DynamicComponentPart<IDictionary>(typeof (T), member);
      dynamicComponentAction(dynamicComponentPart);
      this.providers.Components.Add((IComponentMappingProvider) dynamicComponentPart);
      return dynamicComponentPart;
    }

    /// <summary>
    /// Creates a component reference. This is a place-holder for a component that is defined externally with a
    ///             <see cref="T:FluentNHibernate.Mapping.ComponentMap`1"/>; the mapping defined in said <see cref="T:FluentNHibernate.Mapping.ComponentMap`1"/> will be merged
    ///             with any options you specify from this call.
    /// 
    /// </summary>
    /// <typeparam name="TComponent">Component type</typeparam><param name="member">Property exposing the component</param>
    /// <returns>
    /// Component reference builder
    /// </returns>
    public virtual ReferenceComponentPart<TComponent> Component<TComponent>(Expression<Func<T, TComponent>> member)
    {
      return this.Component<TComponent>(ReflectionExtensions.ToMember<T, TComponent>(member));
    }

    private ReferenceComponentPart<TComponent> Component<TComponent>(Member member)
    {
      this.OnMemberMapped(member);
      ReferenceComponentPart<TComponent> referenceComponentPart = new ReferenceComponentPart<TComponent>(member, typeof (T));
      this.providers.Components.Add((IComponentMappingProvider) referenceComponentPart);
      return referenceComponentPart;
    }

    /// <summary>
    /// Maps a component
    /// 
    /// </summary>
    /// <typeparam name="TComponent">Type of component</typeparam><param name="expression">Component property</param><param name="action">Component mapping</param>
    /// <example>
    /// Component(x =&gt; x.Address, comp =&gt;
    ///             {
    ///               comp.Map(x =&gt; x.Street);
    ///               comp.Map(x =&gt; x.City);
    ///             });
    /// 
    /// </example>
    public ComponentPart<TComponent> Component<TComponent>(Expression<Func<T, TComponent>> expression, Action<ComponentPart<TComponent>> action)
    {
      return this.Component<TComponent>(ReflectionExtensions.ToMember<T, TComponent>(expression), action);
    }

    /// <summary>
    /// Maps a component
    /// 
    /// </summary>
    /// <typeparam name="TComponent">Type of component</typeparam><param name="expression">Component property</param><param name="action">Component mapping</param>
    /// <example>
    /// Component(x =&gt; x.Address, comp =&gt;
    ///             {
    ///               comp.Map(x =&gt; x.Street);
    ///               comp.Map(x =&gt; x.City);
    ///             });
    /// 
    /// </example>
    public ComponentPart<TComponent> Component<TComponent>(Expression<Func<T, object>> expression, Action<ComponentPart<TComponent>> action)
    {
      return this.Component<TComponent>(ReflectionExtensions.ToMember<T, object>(expression), action);
    }

    private ComponentPart<TComponent> Component<TComponent>(Member member, Action<ComponentPart<TComponent>> action)
    {
      this.OnMemberMapped(member);
      ComponentPart<TComponent> componentPart = new ComponentPart<TComponent>(typeof (T), member);
      if (action != null)
        action(componentPart);
      this.providers.Components.Add((IComponentMappingProvider) componentPart);
      return componentPart;
    }

    /// <summary>
    /// Allows the user to add a custom component mapping to the class mapping.
    ///             Note: not a fluent method.
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// In some cases, our users need a way to add an instance of their own implementation of IComponentMappingProvider.
    ///             For an example of where this might be necessary, see: http://codebetter.com/blogs/jeremy.miller/archive/2010/02/16/our-extension-properties-story.aspx
    /// 
    /// </remarks>
    public void Component(IComponentMappingProvider componentProvider)
    {
      this.providers.Components.Add(componentProvider);
    }

    private OneToManyPart<TChild> MapHasMany<TChild, TReturn>(Expression<Func<T, TReturn>> expression)
    {
      return this.HasMany<TChild>(ReflectionExtensions.ToMember<T, TReturn>(expression));
    }

    private OneToManyPart<TChild> HasMany<TChild>(Member member)
    {
      this.OnMemberMapped(member);
      OneToManyPart<TChild> oneToManyPart = new OneToManyPart<TChild>(this.EntityType, member);
      this.providers.Collections.Add((ICollectionMappingProvider) oneToManyPart);
      return oneToManyPart;
    }

    /// <summary>
    /// Maps a collection of entities as a one-to-many
    /// 
    /// </summary>
    /// <typeparam name="TChild">Child entity type</typeparam><param name="memberExpression">Collection property</param>
    /// <example>
    /// HasMany(x =&gt; x.Locations);
    /// 
    /// </example>
    public OneToManyPart<TChild> HasMany<TChild>(Expression<Func<T, IEnumerable<TChild>>> memberExpression)
    {
      return this.MapHasMany<TChild, IEnumerable<TChild>>(memberExpression);
    }

    public OneToManyPart<TChild> HasMany<TKey, TChild>(Expression<Func<T, IDictionary<TKey, TChild>>> memberExpression)
    {
      return this.MapHasMany<TChild, IDictionary<TKey, TChild>>(memberExpression);
    }

    /// <summary>
    /// Maps a collection of entities as a one-to-many
    /// 
    /// </summary>
    /// <typeparam name="TChild">Child entity type</typeparam><param name="memberExpression">Collection property</param>
    /// <example>
    /// HasMany(x =&gt; x.Locations);
    /// 
    /// </example>
    public OneToManyPart<TChild> HasMany<TChild>(Expression<Func<T, object>> memberExpression)
    {
      return this.MapHasMany<TChild, object>(memberExpression);
    }

    private ManyToManyPart<TChild> MapHasManyToMany<TChild, TReturn>(Expression<Func<T, TReturn>> expression)
    {
      return this.HasManyToMany<TChild>(ReflectionExtensions.ToMember<T, TReturn>(expression));
    }

    private ManyToManyPart<TChild> HasManyToMany<TChild>(Member member)
    {
      this.OnMemberMapped(member);
      ManyToManyPart<TChild> manyToManyPart = new ManyToManyPart<TChild>(this.EntityType, member);
      this.providers.Collections.Add((ICollectionMappingProvider) manyToManyPart);
      return manyToManyPart;
    }

    /// <summary>
    /// Maps a collection of entities as a many-to-many
    /// 
    /// </summary>
    /// <typeparam name="TChild">Child entity type</typeparam><param name="memberExpression">Collection property</param>
    /// <example>
    /// HasManyToMany(x =&gt; x.Locations);
    /// 
    /// </example>
    public ManyToManyPart<TChild> HasManyToMany<TChild>(Expression<Func<T, IEnumerable<TChild>>> memberExpression)
    {
      return this.MapHasManyToMany<TChild, IEnumerable<TChild>>(memberExpression);
    }

    /// <summary>
    /// Maps a collection of entities as a many-to-many
    /// 
    /// </summary>
    /// <typeparam name="TChild">Child entity type</typeparam><param name="memberExpression">Collection property</param>
    /// <example>
    /// HasManyToMany(x =&gt; x.Locations);
    /// 
    /// </example>
    public ManyToManyPart<TChild> HasManyToMany<TChild>(Expression<Func<T, object>> memberExpression)
    {
      return this.MapHasManyToMany<TChild, object>(memberExpression);
    }

    /// <summary>
    /// Specify an insert stored procedure
    /// 
    /// </summary>
    /// <param name="innerText">Stored procedure call</param>
    public StoredProcedurePart SqlInsert(string innerText)
    {
      return this.StoredProcedure("sql-insert", innerText);
    }

    /// <summary>
    /// Specify an update stored procedure
    /// 
    /// </summary>
    /// <param name="innerText">Stored procedure call</param>
    public StoredProcedurePart SqlUpdate(string innerText)
    {
      return this.StoredProcedure("sql-update", innerText);
    }

    /// <summary>
    /// Specify an delete stored procedure
    /// 
    /// </summary>
    /// <param name="innerText">Stored procedure call</param>
    public StoredProcedurePart SqlDelete(string innerText)
    {
      return this.StoredProcedure("sql-delete", innerText);
    }

    /// <summary>
    /// Specify an delete all stored procedure
    /// 
    /// </summary>
    /// <param name="innerText">Stored procedure call</param>
    public StoredProcedurePart SqlDeleteAll(string innerText)
    {
      return this.StoredProcedure("sql-delete-all", innerText);
    }

    protected StoredProcedurePart StoredProcedure(string element, string innerText)
    {
      StoredProcedurePart storedProcedurePart = new StoredProcedurePart(element, innerText);
      this.providers.StoredProcedures.Add((IStoredProcedureMappingProvider) storedProcedurePart);
      return storedProcedurePart;
    }
  }
}
