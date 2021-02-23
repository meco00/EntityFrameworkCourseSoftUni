using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeFirstExamFromMSSQL.Models
{
    public class Commits
    {
        //        CREATE TABLE Commits
        //(
        //Id INT IDENTITY PRIMARY KEY,
        //[Message] VARCHAR(255) NOT NULL,
        //IssueId INT FOREIGN KEY REFERENCES Issues(Id),
        //RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
        //ContributorId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL


        public Commits()
        {
            Files = new HashSet<Files>();
        }
        public int Id { get; set; }

        [MaxLength(255)]
        public string Message { get; set; }

        [ForeignKey("Issues")]
        public int IssueId { get; set; }
        public Issues Issue { get; set; }

        [ForeignKey("Repositiories")]
        public int RepositoryId { get; set; }
        public Repositories Repository { get; set; }

        [ForeignKey("Users")]
        public int ContributorId { get; set; }
        public Users User { get; set; }


        public virtual ICollection<Files> Files { get; set; }


    }
}
