using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstExamFromMSSQL.Models
{
    public class Repositories
    {
        //Id INT IDENTITY PRIMARY KEY,
        //Name VARCHAR(50) NOT NULL

        public Repositories()
        {
            Commits = new HashSet<Commits>();
            Issues = new HashSet<Issues>();
            RepositoriesContributors = new HashSet<RepositoriesContributors>();
        }

        public int Id { get; set; }


        [MaxLength(50)]
        public string Name { get; set; }


        public virtual ICollection<Commits> Commits { get; set; }

        public virtual ICollection<Issues> Issues { get; set; }

        public virtual ICollection<RepositoriesContributors> RepositoriesContributors { get; set; }

    }
}