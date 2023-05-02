﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TT.Deliveries.Data;

namespace TT.Deliveries.Data.Migrations
{
    [DbContext(typeof(DeliveryDbContext))]
    partial class DeliveryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("TT.Deliveries.Data.Entities.Delivery", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Delivery");
                });

            modelBuilder.Entity("TT.Deliveries.Data.Entities.Delivery", b =>
                {
                    b.OwnsOne("TT.Deliveries.Data.Entities.AccessWindow", "AccessWindow", b1 =>
                        {
                            b1.Property<Guid>("DeliveryId")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("EndTime")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("StartTime")
                                .HasColumnType("TEXT");

                            b1.HasKey("DeliveryId");

                            b1.ToTable("Delivery");

                            b1.WithOwner()
                                .HasForeignKey("DeliveryId");
                        });

                    b.OwnsOne("TT.Deliveries.Data.Entities.Order", "Order", b1 =>
                        {
                            b1.Property<Guid>("DeliveryId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("OrderNumber")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Sender")
                                .HasColumnType("TEXT");

                            b1.HasKey("DeliveryId");

                            b1.ToTable("Delivery");

                            b1.WithOwner()
                                .HasForeignKey("DeliveryId");
                        });

                    b.OwnsOne("TT.Deliveries.Data.Entities.Recipient", "Recipient", b1 =>
                        {
                            b1.Property<Guid>("DeliveryId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Address")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Email")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Name")
                                .HasColumnType("TEXT");

                            b1.Property<string>("PhoneNumber")
                                .HasColumnType("TEXT");

                            b1.HasKey("DeliveryId");

                            b1.ToTable("Delivery");

                            b1.WithOwner()
                                .HasForeignKey("DeliveryId");
                        });

                    b.Navigation("AccessWindow");

                    b.Navigation("Order");

                    b.Navigation("Recipient");
                });
#pragma warning restore 612, 618
        }
    }
}
