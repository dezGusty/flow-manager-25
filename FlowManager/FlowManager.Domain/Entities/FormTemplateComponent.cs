using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowManager.Domain.Entities
{
    public class FormTemplateComponent
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // navigation properties
        public virtual FormTemplate FormTemplate { get; set; }
        public Guid FormTemplateId { get; set; }

        public virtual Component Component { get; set; }
        public Guid ComponentId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
