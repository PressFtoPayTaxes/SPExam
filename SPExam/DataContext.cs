namespace SPExam
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }

        public DbSet<FileDownloadingInfo> FilesDownloadingInfo { get; set; }
    }
}