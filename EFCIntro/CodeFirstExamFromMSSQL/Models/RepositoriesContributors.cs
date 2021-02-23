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

        
        
        public int RepositoryId { get; set; }
        public virtual Repositories Repository { get; set; }

        
       
        public int ContributorId { get; set; }
        public virtual Users User { get; set; }


    }
}
