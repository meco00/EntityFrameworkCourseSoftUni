using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstExamFromMSSQL.Models
{
    public class Issues
    {
        //        Id INT IDENTITY PRIMARY KEY,
        //Title VARCHAR(255) NOT NULL,
        //IssueStatus VARCHAR(6) NOT NULL,
        //RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
        //AssigneeId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL

        public Issues()
        {
           
        }
        public int Id { get; set; }


        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(6)]
        public string IssueStatus { get; set; }


       
        public int RepositoryId { get; set; }
        public  virtual Repositories Repository { get; set; }


    
        public int AssigneeId { get; set; }
        public  virtual Users User { get; set; }


        public virtual ICollection<Commits> Commits { get; set; }


    }
}