﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TelegramBot.Infrastructure.Data;

#nullable disable

namespace TelegramBot.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230908080357_AddedValueObjectObsession")]
    partial class AddedValueObjectObsession
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("TelegramBot.Domain.Entities.BotMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<float>("Experience")
                        .HasColumnType("REAL");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("TelegramBot.Domain.Entities.BotMember", b =>
                {
                    b.OwnsOne("TelegramBot.Domain.ValueObjects.Account", "Account", b1 =>
                        {
                            b1.Property<Guid>("BotMemberId")
                                .HasColumnType("TEXT");

                            b1.HasKey("BotMemberId");

                            b1.ToTable("Members");

                            b1.WithOwner()
                                .HasForeignKey("BotMemberId");
                        });

                    b.Navigation("Account")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
