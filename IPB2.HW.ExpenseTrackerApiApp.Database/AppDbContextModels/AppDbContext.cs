using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HW.ExpenseTrackerApiApp.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAccount> TblAccounts { get; set; }

    public virtual DbSet<TblBlogDetail> TblBlogDetails { get; set; }

    public virtual DbSet<TblBlogHeader> TblBlogHeaders { get; set; }

    public virtual DbSet<TblBudget> TblBudgets { get; set; }

    public virtual DbSet<TblBurmeseRecipe> TblBurmeseRecipes { get; set; }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblExpense> TblExpenses { get; set; }

    public virtual DbSet<TblStudent> TblStudents { get; set; }

    public virtual DbSet<TblTransferRecord> TblTransferRecords { get; set; }

    public virtual DbSet<TblUserType> TblUserTypes { get; set; }

    public virtual DbSet<TblWallet> TblWallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=IPBatch2;User ID=sa;Password=123@ace;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAccount>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Tbl_Account");

            entity.Property(e => e.AccountId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblBlogDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Tbl_Blog_Details");
        });

        modelBuilder.Entity<TblBlogHeader>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Tbl_Blog_Header");

            entity.Property(e => e.BlogTitle).HasMaxLength(200);
        });

        modelBuilder.Entity<TblBudget>(entity =>
        {
            entity.HasKey(e => e.BudgetId).HasName("PK__Tbl_Budg__E38E79249C4B63DD");

            entity.ToTable("Tbl_Budget");

            entity.HasIndex(e => new { e.BudgetYear, e.BudgetMonth }, "UQ_Budget").IsUnique();

            entity.Property(e => e.BudgetAmount).HasColumnType("decimal(12, 2)");
        });

        modelBuilder.Entity<TblBurmeseRecipe>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("Tbl_Burmese_Recipe");

            entity.Property(e => e.Guid)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CookingInstructions).HasMaxLength(3000);
            entity.Property(e => e.Ingredients).HasMaxLength(3000);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.UserType).WithMany(p => p.TblBurmeseRecipes)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_Burmese_Recipe_Tbl_User_Type");
        });

        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Tbl_Cate__19093A0BAC82E802");

            entity.ToTable("Tbl_Category");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblExpense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PK__Tbl_Expe__1445CFD3999B6C31");

            entity.ToTable("Tbl_Expense");

            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ExpenseDate).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.TblExpenses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expense_Category");
        });

        modelBuilder.Entity<TblStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TblStudent");

            entity.ToTable("Tbl_Student");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.FatherName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblTransferRecord>(entity =>
        {
            entity.HasKey(e => e.TranId);

            entity.ToTable("Tbl_TransferRecord");

            entity.Property(e => e.TranId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.FromMobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.ToMobile)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblUserType>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Tbl_User_Type");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.UserCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.UserEngType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserMmtype)
                .HasMaxLength(50)
                .HasColumnName("UserMMType");
        });

        modelBuilder.Entity<TblWallet>(entity =>
        {
            entity.ToTable("Tbl_Wallet");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
