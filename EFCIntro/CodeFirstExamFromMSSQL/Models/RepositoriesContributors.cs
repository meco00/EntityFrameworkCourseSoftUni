using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeFirstExamFromMSSQL.Models
{
    public class RepositoriesContributors
    {
        //        RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
        //ContributorId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,

        //PRIMARY KEY(RepositoryId, ContributorId)

        
        [ForeignKey("Repositiories")]
        public int RepositoryId { get; set; }
        public Repositories Repository { get; set; }

        
        [ForeignKey("Users")]
        public int ContributorId { get; set; }
        public Users User { get; set; }


    }
}
