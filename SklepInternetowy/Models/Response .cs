﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SklepInternetowy.Models
{
    public class Response
    {
        public bool Sucess { get; set; }
        public string Message { get; set; }
    }
}
