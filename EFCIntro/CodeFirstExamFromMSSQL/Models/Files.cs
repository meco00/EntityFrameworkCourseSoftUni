using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeFirstExamFromMSSQL.Models
{
   public class Files
    {
        //        Id INT IDENTITY PRIMARY KEY,
        //Name VARCHAR(100) NOT NULL,
        //Size DECIMAL(18,2) NOT NULL,
        //ParentId INT FOREIGN KEY REFERENCES Files(Id),
        //CommitId INT FOREIGN KEY REFERENCES Commits(Id) NOT NULL

        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public decimal Size { get; set; }


        [ForeignKey("Files")]
        public int? ParentId { get; set; }
        public Files Parent { get; set; }

        [ForeignKey("Commits")]
        public int CommitId { get; set; }
        public Commits Commit { get; set; }

    }
}
