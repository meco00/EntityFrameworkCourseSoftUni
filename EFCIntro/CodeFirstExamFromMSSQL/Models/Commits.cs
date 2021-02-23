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
           
        }
        public int Id { get; set; }

        [MaxLength(255)]
        public string Message { get; set; }

        
        public int IssueId { get; set; }
        public virtual Issues Issue { get; set; }

       
        public int RepositoryId { get; set; }
        public virtual Repositories Repository { get; set; }

        
        public int ContributorId { get; set; }
        public virtual Users User { get; set; }


        public virtual ICollection<Files> Files { get; set; }


    }
}
