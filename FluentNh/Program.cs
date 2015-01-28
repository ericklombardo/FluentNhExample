using System;
using System.Collections.Generic;
using FluentNh.Entities;
using FluentNh.Mapping;
using FluentNh.Mapping.Convention;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace FluentNh
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cfg = CreateFluentConfiguration();
            var sessionFactory = cfg.BuildSessionFactory();

            Console.WriteLine("Ready");
            Console.ReadKey();
            Console.WriteLine("Inserting data");
            InsertData(sessionFactory);
            Console.WriteLine("Ready");
            Console.ReadKey();
        }

        private static void InsertData(ISessionFactory sessionFactory)
        {
            Client client = new Client
            {
                Name = new Name
                {
                    FirstName = "nombre1",
                    LastName = "apellido"
                },
                Address = new Address
                {
                    City   = "city",
                    Description = "descripcion",
                    State = "ca",
                    Street = "calle"
                }
            };

            Product product = new Product
            {
                Description = "Product1",
                Price = 10
            };

            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(client);
                session.Save(product);

                Invoice invoice = new Invoice
                {
                    Client = client,
                    Code = "code001"
                };

                InvoiceDetail detail = new InvoiceDetail
                {
                    Invoice = invoice,
                    Price = 25,
                    Quantity = 23,
                    Product = product
                };
                
                invoice.Details = new List<InvoiceDetail>
                {
                    detail
                };


                session.Save(invoice);

                transaction.Commit();
            }
        }

        private static FluentConfiguration CreateFluentConfiguration()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(builder => builder.FromConnectionStringWithKey("cnDemo"))
                )
                
                .Mappings(m =>
                    m.AutoMappings.Add(
                        AutoMap
                            .AssemblyOf<Product>(new DemoAutomappingConfiguration())
                            .Conventions.Setup(x =>
                            {
                                x.Add<PrimaryKeyConvention>();
                                x.Add<CustomForeignKeyConvention>();
                                x.Add<ColumnNullableConvention>();
                            })
                            .Override<Invoice>(map=>
                                map.Map(invoice => invoice.Code).Nullable()
                            )
                            .UseOverridesFromAssemblyOf<Product>()
                            )
                        )
                .ExposeConfiguration(cfg =>
                {
                    var exporter = new SchemaExport(cfg);
                    exporter.Drop(false, true);
                    exporter.Create(false, true);
                });
        }
    }
}