using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowManager.Application.DTOs.Requests.FormTemplate
{
    public class PatchFormTemplateRequestDto
    {
        public string? Name { get; set; }
        public string? Content { get; set; }
        public List<Guid>? Components { get; set; }
    }
}
