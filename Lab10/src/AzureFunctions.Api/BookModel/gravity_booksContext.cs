using Microsoft.EntityFrameworkCore;

namespace AzureFunctions.Api.BookModel
{
    public partial class gravity_booksContext : DbContext
    {
        public gravity_booksContext()
        {
        }

        public gravity_booksContext(DbContextOptions<gravity_booksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AddressStatus> AddressStatuses { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookAuthor> BookAuthors { get; set; }
        public virtual DbSet<BookLanguage> BookLanguages { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CustOrder> CustOrders { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public virtual DbSet<OrderHistory> OrderHistories { get; set; }
        public virtual DbSet<OrderLine> OrderLines { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Server=localhost,1433;Database=gravity_books;User Id=SA;Password=quoosugTh5;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.Property(e => e.AddressId)
                    .ValueGeneratedNever()
                    .HasColumnName("address_id");

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.StreetName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("street_name");

                entity.Property(e => e.StreetNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("street_number");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("fk_addr_ctry");
            });

            modelBuilder.Entity<AddressStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("pk_addr_status");

                entity.ToTable("address_status");

                entity.Property(e => e.StatusId)
                    .ValueGeneratedNever()
                    .HasColumnName("status_id");

                entity.Property(e => e.AddressStatus1)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("address_status");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("author");

                entity.Property(e => e.AuthorId)
                    .ValueGeneratedNever()
                    .HasColumnName("author_id");

                entity.Property(e => e.AuthorName)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("author_name");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("book");

                entity.Property(e => e.BookId)
                    .ValueGeneratedNever()
                    .HasColumnName("book_id");

                entity.Property(e => e.Isbn13)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("isbn13");

                entity.Property(e => e.LanguageId).HasColumnName("language_id");

                entity.Property(e => e.NumPages).HasColumnName("num_pages");

                entity.Property(e => e.PublicationDate)
                    .HasColumnType("date")
                    .HasColumnName("publication_date");

                entity.Property(e => e.PublisherId).HasColumnName("publisher_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("fk_book_lang");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("fk_book_pub");
            });

            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.AuthorId })
                    .HasName("pk_bookauthor");

                entity.ToTable("book_author");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ba_author");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ba_book");
            });

            modelBuilder.Entity<BookLanguage>(entity =>
            {
                entity.HasKey(e => e.LanguageId)
                    .HasName("pk_language");

                entity.ToTable("book_language");

                entity.Property(e => e.LanguageId)
                    .ValueGeneratedNever()
                    .HasColumnName("language_id");

                entity.Property(e => e.LanguageCode)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("language_code");

                entity.Property(e => e.LanguageName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("language_name");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("country");

                entity.Property(e => e.CountryId)
                    .ValueGeneratedNever()
                    .HasColumnName("country_id");

                entity.Property(e => e.CountryName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("country_name");
            });

            modelBuilder.Entity<CustOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("pk_custorder");

                entity.ToTable("cust_order");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.DestAddressId).HasColumnName("dest_address_id");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date");

                entity.Property(e => e.ShippingMethodId).HasColumnName("shipping_method_id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("fk_order_cust");

                entity.HasOne(d => d.DestAddress)
                    .WithMany(p => p.CustOrders)
                    .HasForeignKey(d => d.DestAddressId)
                    .HasConstraintName("fk_order_addr");

                entity.HasOne(d => d.ShippingMethod)
                    .WithMany(p => p.CustOrders)
                    .HasForeignKey(d => d.ShippingMethodId)
                    .HasConstraintName("fk_order_ship");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.Property(e => e.CustomerId)
                    .ValueGeneratedNever()
                    .HasColumnName("customer_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(350)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("last_name");
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.AddressId })
                    .HasName("pk_custaddr");

                entity.ToTable("customer_address");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.CustomerAddresses)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ca_addr");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerAddresses)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ca_cust");
            });

            modelBuilder.Entity<OrderHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId)
                    .HasName("pk_orderhist");

                entity.ToTable("order_history");

                entity.Property(e => e.HistoryId).HasColumnName("history_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.StatusDate)
                    .HasColumnType("datetime")
                    .HasColumnName("status_date");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderHistories)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("fk_oh_order");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.OrderHistories)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("fk_oh_status");
            });

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.HasKey(e => e.LineId)
                    .HasName("pk_orderline");

                entity.ToTable("order_line");

                entity.Property(e => e.LineId).HasColumnName("line_id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("price");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.OrderLines)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("fk_ol_book");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLines)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("fk_ol_order");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("pk_orderstatus");

                entity.ToTable("order_status");

                entity.Property(e => e.StatusId)
                    .ValueGeneratedNever()
                    .HasColumnName("status_id");

                entity.Property(e => e.StatusValue)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("status_value");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.ToTable("publisher");

                entity.Property(e => e.PublisherId)
                    .ValueGeneratedNever()
                    .HasColumnName("publisher_id");

                entity.Property(e => e.PublisherName)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("publisher_name");
            });

            modelBuilder.Entity<ShippingMethod>(entity =>
            {
                entity.HasKey(e => e.MethodId)
                    .HasName("pk_shipmethod");

                entity.ToTable("shipping_method");

                entity.Property(e => e.MethodId)
                    .ValueGeneratedNever()
                    .HasColumnName("method_id");

                entity.Property(e => e.Cost)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("cost");

                entity.Property(e => e.MethodName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("method_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
