using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CodeFirstExamFromMSSQL.Models
{
   public class Users
    {

        public Users()
        {
            Commits = new HashSet<Commits>();
            Issues = new HashSet<Issues>();
            RepositoriesContributors = new HashSet<RepositoriesContributors>();
        }
        public int Id { get; set; }

        [MaxLength(30)]
        public string Username { get; set; }

        [MaxLength(30)]
        public string Password { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public virtual ICollection<Commits> Commits { get; set; }

        public virtual ICollection<Issues> Issues { get; set; }

        public virtual ICollection<RepositoriesContributors> RepositoriesContributors { get; set; }

    }
}
