using System;
using Microsoft.AspNetCore.Mvc;

namespace ticketApp.Models.Utility;

static class ToastMethod
{
    public static void ToastMessage(this Controller cn,string message)
    {
        cn.ViewData["ToastMessage"] = message;
    }
}
