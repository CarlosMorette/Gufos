﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Back_end.Models {
    public partial class GufosContext : DbContext {
        public GufosContext () { }

        public GufosContext (DbContextOptions<GufosContext> options) : base (options) { }

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Evento> Evento { get; set; }
        public virtual DbSet<Localizacao> Localizacao { get; set; }
        public virtual DbSet<Presenca> Presenca { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer ("Server=DESKTOP-RPPJ3RD\\SQLEXPRESS; Database=Gufos; User Id=sa; Password=132");
            }
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.Entity<Categoria> (entity => {
                entity.HasIndex (e => e.Titulo)
                    .HasName ("UQ__Categori__7B406B56D37946FE")
                    .IsUnique ();

                entity.Property (e => e.Titulo).IsUnicode (false);
            });

            modelBuilder.Entity<Evento> (entity => {
                entity.Property (e => e.AcessoLivre).HasDefaultValueSql ("((1))");

                entity.Property (e => e.Titulo).IsUnicode (false);

                entity.HasOne (d => d.Categoria)
                    .WithMany (p => p.Evento)
                    .HasForeignKey (d => d.CategoriaId)
                    .HasConstraintName ("FK__Evento__Categori__44FF419A");

                entity.HasOne (d => d.Localizacao)
                    .WithMany (p => p.Evento)
                    .HasForeignKey (d => d.LocalizacaoId)
                    .HasConstraintName ("FK__Evento__Localiza__46E78A0C");
            });

            modelBuilder.Entity<Localizacao> (entity => {
                entity.HasIndex (e => e.Cnpj)
                    .HasName ("UQ__Localiza__AA57D6B4FCB2A620")
                    .IsUnique ();

                entity.HasIndex (e => e.RazaoSocial)
                    .HasName ("UQ__Localiza__B0E5930EC7744541")
                    .IsUnique ();

                entity.Property (e => e.Cnpj)
                    .IsUnicode (false)
                    .IsFixedLength ();

                entity.Property (e => e.Endereco).IsUnicode (false);

                entity.Property (e => e.RazaoSocial).IsUnicode (false);
            });

            modelBuilder.Entity<Presenca> (entity => {
                entity.Property (e => e.PresencaStatus).IsUnicode (false);

                entity.HasOne (d => d.Evento)
                    .WithMany (p => p.Presenca)
                    .HasForeignKey (d => d.EventoId)
                    .HasConstraintName ("FK__Presenca__Evento__49C3F6B7");
                    
                entity.HasOne (d => d.Usuario)
                    .WithMany (p => p.Presenca)
                    .HasForeignKey (d => d.UsuarioId)
                    .HasConstraintName ("FK__Presenca__Usuari__4AB81AF0");
            });

            modelBuilder.Entity<TipoUsuario> (entity => {
                entity.HasIndex (e => e.Titulo)
                    .HasName ("UQ__Tipo_Usu__7B406B565A97EDA9")
                    .IsUnique ();

                entity.Property (e => e.Titulo).IsUnicode (false);
            });

            modelBuilder.Entity<Usuario> (entity => {
                entity.HasIndex (e => e.Email)
                    .HasName ("UQ__Usuario__A9D10534AED4C299")
                    .IsUnique ();

                entity.Property (e => e.Email).IsUnicode (false);

                entity.Property (e => e.Nome).IsUnicode (false);

                entity.Property (e => e.Senha).IsUnicode (false);

                entity.HasOne (d => d.TipoUsuario)
                    .WithMany (p => p.Usuario)
                    .HasForeignKey (d => d.TipoUsuarioId)
                    .HasConstraintName ("FK__Usuario__Tipo_Us__3B75D760");
            });

            OnModelCreatingPartial (modelBuilder);
        }

        partial void OnModelCreatingPartial (ModelBuilder modelBuilder);
    }
}