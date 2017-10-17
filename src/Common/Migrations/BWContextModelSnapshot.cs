﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BioEngine.Common.DB;

namespace BioEngine.Common.Migrations
{
    [DbContext(typeof(BWContext))]
    partial class BWContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("BioEngine.Common.Models.AccessToken", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("access_token");

                    b.Property<string>("ClientId")
                        .HasColumnName("client_id");

                    b.Property<DateTime>("Expires")
                        .HasColumnName("expires");

                    b.Property<int>("MemberId")
                        .HasColumnName("member_id");

                    b.Property<string>("Scope")
                        .HasColumnName("scope");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("be_oauth2server_access_tokens");
                });

            modelBuilder.Entity("BioEngine.Common.Models.Advertisement", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ad_id");

                    b.Property<int>("AdActive")
                        .HasColumnName("ad_active");

                    b.Property<string>("AdAdditionalSettings")
                        .HasColumnName("ad_additional_settings");

                    b.Property<int>("AdClicks")
                        .HasColumnName("ad_clicks");

                    b.Property<int>("AdEnd")
                        .HasColumnName("ad_end");

                    b.Property<string>("AdExempt")
                        .HasColumnName("ad_exempt");

                    b.Property<string>("AdHtml")
                        .HasColumnName("ad_html");

                    b.Property<string>("AdHtmlHttps")
                        .HasColumnName("ad_html_https");

                    b.Property<bool>("AdHtmlHttpsSet")
                        .HasColumnName("ad_html_https_set");

                    b.Property<string>("AdImages")
                        .HasColumnName("ad_images");

                    b.Property<ulong>("AdImpressions")
                        .HasColumnName("ad_impressions");

                    b.Property<string>("AdLink")
                        .HasColumnName("ad_link");

                    b.Property<string>("AdLocation")
                        .HasColumnName("ad_location");

                    b.Property<string>("AdMaximumUnit")
                        .HasColumnName("ad_maximum_unit");

                    b.Property<int>("AdMaximumValue")
                        .HasColumnName("ad_maximum_value");

                    b.Property<uint?>("AdMember")
                        .HasColumnName("ad_member");

                    b.Property<int>("AdStart")
                        .HasColumnName("ad_start");

                    b.HasKey("Id");

                    b.ToTable("be_core_advertisements");
                });

            modelBuilder.Entity("BioEngine.Common.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Announce")
                        .HasColumnName("announce");

                    b.Property<int>("AuthorId")
                        .HasColumnName("author_id");

                    b.Property<int?>("CatId")
                        .HasColumnName("cat_id");

                    b.Property<int>("Count")
                        .HasColumnName("count");

                    b.Property<int>("Date")
                        .HasColumnName("date");

                    b.Property<int?>("DeveloperId")
                        .HasColumnName("developer_id");

                    b.Property<int>("Fs")
                        .HasColumnName("fs");

                    b.Property<int?>("GameId")
                        .HasColumnName("game_id");

                    b.Property<int>("Pub")
                        .HasColumnName("pub");

                    b.Property<string>("Source")
                        .HasColumnName("source");

                    b.Property<string>("Text")
                        .HasColumnName("text");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.Property<int?>("TopicId")
                        .HasColumnName("topic_id");

                    b.Property<string>("Url")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CatId");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("GameId");

                    b.HasIndex("TopicId");

                    b.ToTable("be_articles");
                });

            modelBuilder.Entity("BioEngine.Common.Models.ArticleCat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("Articles")
                        .HasColumnName("articles");

                    b.Property<int?>("CatId")
                        .HasColumnName("pid");

                    b.Property<string>("Content")
                        .HasColumnName("content");

                    b.Property<string>("Descr")
                        .HasColumnName("descr");

                    b.Property<int?>("DeveloperId")
                        .HasColumnName("developer_id");

                    b.Property<int?>("GameId")
                        .HasColumnName("game_id");

                    b.Property<string>("GameOld")
                        .HasColumnName("game_old");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.Property<int?>("TopicId")
                        .HasColumnName("topic_id");

                    b.Property<string>("Url")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.HasIndex("CatId");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("GameId");

                    b.HasIndex("TopicId");

                    b.ToTable("be_articles_cats");
                });

            modelBuilder.Entity("BioEngine.Common.Models.Block", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("index");

                    b.Property<int>("Active")
                        .HasColumnName("active");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnName("content");

                    b.HasKey("Id");

                    b.ToTable("be_blocks");
                });

            modelBuilder.Entity("BioEngine.Common.Models.Developer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Desc")
                        .HasColumnName("desc");

                    b.Property<int>("FoundYear")
                        .HasColumnName("found_year");

                    b.Property<string>("Info")
                        .HasColumnName("info");

                    b.Property<string>("Location")
                        .HasColumnName("location");

                    b.Property<string>("Logo")
                        .HasColumnName("logo");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<string>("Peoples")
                        .HasColumnName("peoples");

                    b.Property<int>("RateNeg")
                        .HasColumnName("rate_neg");

                    b.Property<int>("RatePos")
                        .HasColumnName("rate_pos");

                    b.Property<string>("Site")
                        .HasColumnName("site");

                    b.Property<string>("Url")
                        .HasColumnName("url");

                    b.Property<string>("VotedUsers")
                        .HasColumnName("voted_users");

                    b.HasKey("Id");

                    b.ToTable("be_developers");
                });

            modelBuilder.Entity("BioEngine.Common.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Announce")
                        .HasColumnName("announce");

                    b.Property<int>("AuthorId")
                        .HasColumnName("author_id");

                    b.Property<int>("CatId")
                        .HasColumnName("cat_id");

                    b.Property<int>("Count")
                        .HasColumnName("count");

                    b.Property<int>("Date")
                        .HasColumnName("date");

                    b.Property<string>("Desc")
                        .HasColumnName("desc");

                    b.Property<int?>("DeveloperId")
                        .HasColumnName("developer_id");

                    b.Property<int?>("GameId")
                        .HasColumnName("game_id");

                    b.Property<string>("Link")
                        .HasColumnName("link");

                    b.Property<int>("Size")
                        .HasColumnName("size");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.Property<string>("Url")
                        .HasColumnName("url");

                    b.Property<string>("YtId")
                        .HasColumnName("yt_id");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CatId");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("GameId");

                    b.ToTable("be_files");
                });

            modelBuilder.Entity("BioEngine.Common.Models.FileCat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int?>("CatId")
                        .HasColumnName("pid");

                    b.Property<string>("Descr")
                        .HasColumnName("descr");

                    b.Property<int?>("DeveloperId")
                        .HasColumnName("developer_id");

                    b.Property<int?>("GameId")
                        .HasColumnName("game_id");

                    b.Property<string>("GameOld")
                        .HasColumnName("game_old");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.Property<string>("Url")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.HasIndex("CatId");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("GameId");

                    b.ToTable("be_files_cats");
                });

            modelBuilder.Entity("BioEngine.Common.Models.GalleryCat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Desc")
                        .HasColumnName("desc");

                    b.Property<int?>("DeveloperId")
                        .HasColumnName("developer_id");

                    b.Property<int?>("GameId")
                        .HasColumnName("game_id");

                    b.Property<string>("GameOld")
                        .HasColumnName("game_old");

                    b.Property<int?>("Pid")
                        .HasColumnName("pid");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.Property<string>("Url")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("GameId");

                    b.HasIndex("Pid");

                    b.ToTable("be_gallery_cats");
                });

            modelBuilder.Entity("BioEngine.Common.Models.GalleryPic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("CatId")
                        .HasColumnName("cat_id");

                    b.Property<string>("Desc")
                        .HasColumnName("desc");

                    b.Property<int?>("DeveloperId")
                        .HasColumnName("developer_id");

                    b.Property<string>("FilesJson")
                        .HasColumnName("files");

                    b.Property<int?>("GameId")
                        .HasColumnName("game_id");

                    b.Property<int>("Pub")
                        .HasColumnName("pub");

                    b.HasKey("Id");

                    b.HasIndex("CatId");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("GameId");

                    b.ToTable("be_gallery");
                });

            modelBuilder.Entity("BioEngine.Common.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("AdminTitle")
                        .HasColumnName("admin_title");

                    b.Property<int>("Date")
                        .HasColumnName("date");

                    b.Property<string>("Desc")
                        .HasColumnName("desc");

                    b.Property<string>("Dev")
                        .HasColumnName("dev");

                    b.Property<int>("DeveloperId")
                        .HasColumnName("developer_id");

                    b.Property<string>("Genre")
                        .HasColumnName("genre");

                    b.Property<string>("Info")
                        .HasColumnName("info");

                    b.Property<string>("Keywords")
                        .HasColumnName("keywords");

                    b.Property<string>("Localizator")
                        .HasColumnName("localizator");

                    b.Property<string>("Logo")
                        .HasColumnName("logo");

                    b.Property<string>("NewsDesc")
                        .HasColumnName("news_desc");

                    b.Property<string>("Platforms")
                        .HasColumnName("platforms");

                    b.Property<string>("Publisher")
                        .HasColumnName("publisher");

                    b.Property<int>("RateNeg")
                        .HasColumnName("rate_neg");

                    b.Property<int>("RatePos")
                        .HasColumnName("rate_pos");

                    b.Property<string>("ReleaseDate")
                        .HasColumnName("release_date");

                    b.Property<string>("SmallLogo")
                        .HasColumnName("small_logo");

                    b.Property<string>("Specs")
                        .HasColumnName("specs");

                    b.Property<int>("Status")
                        .HasColumnName("status");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.Property<string>("TweetTag")
                        .HasColumnName("tweettag");

                    b.Property<string>("Url")
                        .HasColumnName("url");

                    b.Property<string>("VotedUsers")
                        .HasColumnName("voted_users");

                    b.HasKey("Id");

                    b.HasIndex("DeveloperId");

                    b.ToTable("be_games");
                });

            modelBuilder.Entity("BioEngine.Common.Models.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .HasColumnName("code");

                    b.Property<int>("Date")
                        .HasColumnName("date");

                    b.Property<string>("Key")
                        .HasColumnName("key");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("be_menu");
                });

            modelBuilder.Entity("BioEngine.Common.Models.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("AddGames")
                        .HasColumnName("addgames");

                    b.Property<string>("AddText")
                        .HasColumnName("add_text");

                    b.Property<int>("AuthorId")
                        .HasColumnName("author_id");

                    b.Property<int>("Comments")
                        .HasColumnName("comments");

                    b.Property<long>("Date")
                        .HasColumnName("date");

                    b.Property<int?>("DeveloperId")
                        .HasColumnName("developer_id");

                    b.Property<int?>("ForumPostId")
                        .HasColumnName("pid");

                    b.Property<int?>("ForumTopicId")
                        .HasColumnName("tid");

                    b.Property<int?>("GameId")
                        .HasColumnName("game_id");

                    b.Property<string>("GameOld")
                        .HasColumnName("game_old");

                    b.Property<long>("LastChangeDate")
                        .HasColumnName("last_change_date");

                    b.Property<int>("Pub")
                        .HasColumnName("pub");

                    b.Property<int>("RateNeg")
                        .HasColumnName("rate_neg");

                    b.Property<int>("RatePos")
                        .HasColumnName("rate_pos");

                    b.Property<string>("ShortText")
                        .HasColumnName("short_text");

                    b.Property<string>("Source")
                        .HasColumnName("source");

                    b.Property<int>("Sticky")
                        .HasColumnName("sticky");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.Property<int?>("TopicId")
                        .HasColumnName("topic_id");

                    b.Property<long>("TwitterId")
                        .HasColumnName("twitter_id");

                    b.Property<string>("Url")
                        .HasColumnName("url");

                    b.Property<string>("VotedUsers")
                        .HasColumnName("voted_users");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("GameId");

                    b.HasIndex("TopicId");

                    b.ToTable("be_news");
                });

            modelBuilder.Entity("BioEngine.Common.Models.Poll", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("poll_id");

                    b.Property<int>("Multiple")
                        .HasColumnName("multiple");

                    b.Property<int>("NumChoices")
                        .HasColumnName("num_choices");

                    b.Property<int>("OnOff")
                        .HasColumnName("onoff");

                    b.Property<string>("OptionsJson")
                        .HasColumnName("options");

                    b.Property<string>("Question")
                        .HasColumnName("question");

                    b.Property<int>("StartDate")
                        .HasColumnName("startdate");

                    b.Property<string>("VotesJson")
                        .HasColumnName("votes");

                    b.HasKey("Id");

                    b.ToTable("be_poll");
                });

            modelBuilder.Entity("BioEngine.Common.Models.PollWho", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("poll_who_id");

                    b.Property<string>("Ip")
                        .HasColumnName("ip");

                    b.Property<string>("Login")
                        .HasColumnName("login");

                    b.Property<int>("PollId")
                        .HasColumnName("poll_id");

                    b.Property<string>("SessionId")
                        .HasColumnName("session_id");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.Property<long>("VoteDate")
                        .HasColumnName("vote_date");

                    b.Property<int>("VoteOption")
                        .HasColumnName("voteoption");

                    b.HasKey("Id");

                    b.ToTable("be_poll_who");
                });

            modelBuilder.Entity("BioEngine.Common.Models.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Desc")
                        .HasColumnName("desc");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.Property<string>("Type")
                        .HasColumnName("type");

                    b.Property<string>("Value")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.ToTable("be_settings");
                });

            modelBuilder.Entity("BioEngine.Common.Models.SiteTeamMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("Active")
                        .HasColumnName("active");

                    b.Property<int>("Articles")
                        .HasColumnName("articles");

                    b.Property<int>("Developers")
                        .HasColumnName("developers");

                    b.Property<int>("Files")
                        .HasColumnName("files");

                    b.Property<int>("Gallery")
                        .HasColumnName("gallery");

                    b.Property<int>("Games")
                        .HasColumnName("games");

                    b.Property<int>("MemberId")
                        .HasColumnName("member_id");

                    b.Property<int>("News")
                        .HasColumnName("news");

                    b.Property<int>("Polls")
                        .HasColumnName("polls");

                    b.Property<int>("Tags")
                        .HasColumnName("tags");

                    b.HasKey("Id");

                    b.HasIndex("MemberId")
                        .IsUnique();

                    b.ToTable("be_site_team");
                });

            modelBuilder.Entity("BioEngine.Common.Models.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Desc")
                        .HasColumnName("desc");

                    b.Property<string>("Logo")
                        .HasColumnName("logo");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.Property<string>("Url")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.ToTable("be_nuke_topics");
                });

            modelBuilder.Entity("BioEngine.Common.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("member_id");

                    b.Property<int>("GroupId")
                        .HasColumnName("member_group_id");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("be_core_members");
                });

            modelBuilder.Entity("BioEngine.Common.Models.AccessToken", b =>
                {
                    b.HasOne("BioEngine.Common.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BioEngine.Common.Models.Article", b =>
                {
                    b.HasOne("BioEngine.Common.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BioEngine.Common.Models.ArticleCat", "Cat")
                        .WithMany("Items")
                        .HasForeignKey("CatId");

                    b.HasOne("BioEngine.Common.Models.Developer", "Developer")
                        .WithMany()
                        .HasForeignKey("DeveloperId");

                    b.HasOne("BioEngine.Common.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("BioEngine.Common.Models.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId");
                });

            modelBuilder.Entity("BioEngine.Common.Models.ArticleCat", b =>
                {
                    b.HasOne("BioEngine.Common.Models.ArticleCat", "ParentCat")
                        .WithMany("Children")
                        .HasForeignKey("CatId");

                    b.HasOne("BioEngine.Common.Models.Developer", "Developer")
                        .WithMany()
                        .HasForeignKey("DeveloperId");

                    b.HasOne("BioEngine.Common.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("BioEngine.Common.Models.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId");
                });

            modelBuilder.Entity("BioEngine.Common.Models.File", b =>
                {
                    b.HasOne("BioEngine.Common.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BioEngine.Common.Models.FileCat", "Cat")
                        .WithMany("Items")
                        .HasForeignKey("CatId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BioEngine.Common.Models.Developer", "Developer")
                        .WithMany()
                        .HasForeignKey("DeveloperId");

                    b.HasOne("BioEngine.Common.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("BioEngine.Common.Models.FileCat", b =>
                {
                    b.HasOne("BioEngine.Common.Models.FileCat", "ParentCat")
                        .WithMany("Children")
                        .HasForeignKey("CatId");

                    b.HasOne("BioEngine.Common.Models.Developer", "Developer")
                        .WithMany()
                        .HasForeignKey("DeveloperId");

                    b.HasOne("BioEngine.Common.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("BioEngine.Common.Models.GalleryCat", b =>
                {
                    b.HasOne("BioEngine.Common.Models.Developer", "Developer")
                        .WithMany()
                        .HasForeignKey("DeveloperId");

                    b.HasOne("BioEngine.Common.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("BioEngine.Common.Models.GalleryCat", "ParentCat")
                        .WithMany("Children")
                        .HasForeignKey("Pid");
                });

            modelBuilder.Entity("BioEngine.Common.Models.GalleryPic", b =>
                {
                    b.HasOne("BioEngine.Common.Models.GalleryCat", "Cat")
                        .WithMany("Items")
                        .HasForeignKey("CatId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BioEngine.Common.Models.Developer", "Developer")
                        .WithMany()
                        .HasForeignKey("DeveloperId");

                    b.HasOne("BioEngine.Common.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("BioEngine.Common.Models.Game", b =>
                {
                    b.HasOne("BioEngine.Common.Models.Developer", "Developer")
                        .WithMany()
                        .HasForeignKey("DeveloperId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BioEngine.Common.Models.News", b =>
                {
                    b.HasOne("BioEngine.Common.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BioEngine.Common.Models.Developer", "Developer")
                        .WithMany()
                        .HasForeignKey("DeveloperId");

                    b.HasOne("BioEngine.Common.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("BioEngine.Common.Models.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId");
                });

            modelBuilder.Entity("BioEngine.Common.Models.SiteTeamMember", b =>
                {
                    b.HasOne("BioEngine.Common.Models.User", "User")
                        .WithOne("SiteTeamMember")
                        .HasForeignKey("BioEngine.Common.Models.SiteTeamMember", "MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
