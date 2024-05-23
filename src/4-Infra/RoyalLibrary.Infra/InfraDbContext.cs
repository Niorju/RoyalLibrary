using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using RoyalLibrary.Domain.Entities;

namespace RoyalLibrary.Infra
{
    public class InfraDbContext : DbContext, IBookRepositorie
    {
        public InfraDbContext(DbContextOptions<InfraDbContext> options)
          : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Ignore(t => t.ValidationResult);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.EnableSensitiveDataLogging();
        }

        public async Task<List<Book>> GetBooks()
        {
            try
            {
                var books = Books.ToList();
                var query = from book in books select book;                
                return await Task.FromResult(query.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }      
        public async Task<Book> GetBookById(int id)
        {
            try
            {
                var bookbyId = Books.ToList();
                var query = from book in bookbyId where book.BookId == id select book;
                return await Task.FromResult(query.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Book>> GetBookByCategory(string category)
        {
            try
            {
                var books = Books.ToList();
                var query = from book in books where book.Category == category select book;
                               
                return await Task.FromResult(query.ToList());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> UpdateBookAsync(Book entitie)
        {
            try
            {

                var local = Set<Book>().Local.FirstOrDefault(entry => entry.BookId.Equals(entitie.BookId));
                if (local != null)
                {
                    Entry(local).State = EntityState.Detached;
                }
                Entry(entitie).State = EntityState.Modified;

                await SaveChangesAsync();
                               
                return entitie.Title;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> InsertBookAsync(Book entitie)
        {
            try
            {
                var newID = Books.Select(x => x.BookId).Max() + 1;
                entitie.BookId = newID;
                Books.Add(entitie);
                await SaveChangesAsync();
                
                return entitie.Title;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }       
    }
}
