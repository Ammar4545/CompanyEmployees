﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Incoming
{
    public record EmployeeForCreationDto(string Name, int Age, string Position);
}
