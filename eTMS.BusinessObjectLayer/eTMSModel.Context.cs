﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eTMS.BusinessObjectLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class eTMSEntities : DbContext
    {
        public eTMSEntities()
            : base("name=eTMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccountTable> AccountTables { get; set; }
        public virtual DbSet<DealerTable> DealerTables { get; set; }
        public virtual DbSet<TransactionTypeTable> TransactionTypeTables { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<TransactionTable> TransactionTables { get; set; }
        public virtual DbSet<TransactionFactorTable> TransactionFactorTables { get; set; }
        public virtual DbSet<GsmValidator> GsmValidators { get; set; }
        public virtual DbSet<Debtor> Debtors { get; set; }
        public virtual DbSet<Debt> Debts { get; set; }
        public virtual DbSet<ProfitAndLoss> ProfitAndLosses { get; set; }
        public virtual DbSet<EventLog> EventLogs { get; set; }
        public virtual DbSet<Setup> Setups { get; set; }
        public virtual DbSet<Annual> Annuals { get; set; }
    }
}
