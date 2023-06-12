using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Estore.Core.Entities
{
    public class Setting : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Site Başlık")]
        public string? Title { get; set; }
        [Display(Name = "Site Açıklama")]
        public string? Description { get; set; }
        public string? Email { get; set; }
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }
        [Display(Name = "Mail Sunucusu")]
        public string? MailServer { get; set; }
        public int? Port { get; set; }
        [Display(Name = "Kullanıcı Adı")]
        public string? Username { get; set; }
        [Display(Name = "Şifre")]
        public string? Password { get; set; }
        [Display(Name = "Sekme Logo")]
        public string? Favicon { get; set; }
        [Display(Name = "Site Logosu")]
        public string? Logo { get; set; }


    }
}
