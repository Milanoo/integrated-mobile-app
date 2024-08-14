//--------------------------------------------------------------------
//   Date                 : November, 2023
//    Copyright: (C)2023 by AHT-GROUP, GmbH/SOFTWEL (P) Ltd
//    Email                : support at softwel dot com dot np 
// --------------------------------------------------------------------

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation (https://www.fsf.org/); either version 2
// of the License, or (at your option) any later version.

using System;
using System.Collections.Generic;
using IntegratedMobileApp.Helpers;
using Microsoft.EntityFrameworkCore;

namespace IntegratedMobileApp.Models.Database;

public partial class ImobappContext : DbContext
{
    public ImobappContext()
    {
    }

    public ImobappContext(DbContextOptions<ImobappContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TabAttachment> TabAttachments { get; set; }

    public virtual DbSet<TabInfoUrl> TabInfoUrls { get; set; }

    public virtual DbSet<TabInfotouser> TabInfotousers { get; set; }

    public virtual DbSet<TabLink> TabLinks { get; set; }

    public virtual DbSet<TabPoll> TabPolls { get; set; }

    public virtual DbSet<TabPollVote> TabPollVotes { get; set; }

    public virtual DbSet<TabPretext> TabPretexts { get; set; }

    public virtual DbSet<TabUser> TabUsers { get; set; }

    public virtual DbSet<TabUsergriv> TabUsergrivs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(AppConfiguration.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TabAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tab_attachment_pkey");

            entity.ToTable("tab_attachment", "system");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AId).HasColumnName("a_id");
            entity.Property(e => e.Descr).HasColumnName("descr");
        });

        modelBuilder.Entity<TabInfoUrl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tab_info_url_pkey");

            entity.ToTable("tab_info_url", "system");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArticleUrl).HasColumnName("article_url");
            entity.Property(e => e.ElectedUrl).HasColumnName("elected_url");
            entity.Property(e => e.HnUrl).HasColumnName("hn_url");
            entity.Property(e => e.ImaUrl).HasColumnName("ima_url");
            entity.Property(e => e.IntroUrl).HasColumnName("intro_url");
            entity.Property(e => e.MunCode).HasColumnName("mun_code");
            entity.Property(e => e.NbUrl).HasColumnName("nb_url");
            entity.Property(e => e.NbYr).HasColumnName("nb_yr");
            entity.Property(e => e.PmUrl).HasColumnName("pm_url");
            entity.Property(e => e.SifarisUrl).HasColumnName("sifaris_url");
            entity.Property(e => e.StaffUrl).HasColumnName("staff_url");
        });

        modelBuilder.Entity<TabInfotouser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tab_infotouser_pkey");

            entity.ToTable("tab_infotouser", "system");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Infoid).HasColumnName("infoid");
            entity.Property(e => e.Inforeceiver).HasColumnName("inforeceiver");
            entity.Property(e => e.Infosender).HasColumnName("infosender");
            entity.Property(e => e.Infotext).HasColumnName("infotext");
        });

        modelBuilder.Entity<TabLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tab_link_pkey");

            entity.ToTable("tab_link", "system");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Linkdetail).HasColumnName("linkdetail");
            entity.Property(e => e.Lkid).HasColumnName("lkid");
            entity.Property(e => e.Lnameeng).HasColumnName("lnameeng");
            entity.Property(e => e.Lnamenep).HasColumnName("lnamenep");
        });

        modelBuilder.Entity<TabPoll>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tab_poll_pkey");

            entity.ToTable("tab_poll", "system");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MunCode).HasColumnName("mun_code");
            entity.Property(e => e.OText).HasColumnName("o_text");
            entity.Property(e => e.OUuid).HasColumnName("o_uuid");
            entity.Property(e => e.QText).HasColumnName("q_text");
            entity.Property(e => e.QUuid).HasColumnName("q_uuid");
        });

        modelBuilder.Entity<TabPollVote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tab_poll_vote_pkey");

            entity.ToTable("tab_poll_vote", "system");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OUuid).HasColumnName("o_uuid");
            entity.Property(e => e.Umail).HasColumnName("umail");
            entity.Property(e => e.VotedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("voted_on");
        });

        modelBuilder.Entity<TabPretext>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tab_pretext_pkey");

            entity.ToTable("tab_pretext", "system");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Engtext).HasColumnName("engtext");
            entity.Property(e => e.Neptext).HasColumnName("neptext");
            entity.Property(e => e.Usertype).HasColumnName("usertype");
        });

        modelBuilder.Entity<TabUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tab_user_pkey");

            entity.ToTable("tab_user", "system");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.Fctz).HasColumnName("fctz");
            entity.Property(e => e.Fname).HasColumnName("fname");
            entity.Property(e => e.Mctz).HasColumnName("mctz");
            entity.Property(e => e.Mname).HasColumnName("mname");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Uctz).HasColumnName("uctz");
            entity.Property(e => e.Uhno).HasColumnName("uhno");
            entity.Property(e => e.Umail).HasColumnName("umail");
            entity.Property(e => e.Uname).HasColumnName("uname");
            entity.Property(e => e.Uphone).HasColumnName("uphone");
            entity.Property(e => e.Upmw).HasColumnName("upmw");
            entity.Property(e => e.Utype).HasColumnName("utype");
        });

        modelBuilder.Entity<TabUsergriv>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tab_usergriv_pkey");

            entity.ToTable("tab_usergriv", "system");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Gpic).HasColumnName("gpic");
            entity.Property(e => e.Grivid).HasColumnName("grivid");
            entity.Property(e => e.Grivreceiver).HasColumnName("grivreceiver");
            entity.Property(e => e.Grivsender).HasColumnName("grivsender");
            entity.Property(e => e.Grivtext).HasColumnName("grivtext");
            entity.Property(e => e.Replyid).HasColumnName("replyid");
            entity.Property(e => e.Replytext).HasColumnName("replytext");
            entity.Property(e => e.Rpic).HasColumnName("rpic");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
