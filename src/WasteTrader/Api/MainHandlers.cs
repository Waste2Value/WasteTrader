﻿using Starcounter;
using WasteTrader.ViewModels;

namespace WasteTrader.Api
{
    class MainHandlers : IHandler
    {
        public void Register()
        {
            var html = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
    <title>{0}</title>
    <script src=""/sys/webcomponentsjs/webcomponents.min.js""></script>
    <link rel=""import"" href=""/sys/polymer/polymer.html"">
    <link rel=""import"" href=""/sys/starcounter.html"">
    <link rel=""import"" href=""/sys/starcounter-include/starcounter-include.html"">
    <link rel=""import"" href=""/sys/starcounter-debug-aid/src/starcounter-debug-aid.html"">
    <link rel=""stylesheet"" href=""/sys/material-components-web.min.css"">
</head>
<body>
    <template is=""dom-bind"" id=""puppet-root"">
        <template is=""imported-template"" content$=""{{{{model.Html}}}}"" model=""{{{{model}}}}""></template>
    </template>
    <puppet-client ref=""puppet-root"" remote-url=""{1}""></puppet-client>
    <starcounter-debug-aid></starcounter-debug-aid>
    <script src=""/sys/material-components-web.min.js""></script>
 </body>
</html>";

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider(html));

            Handle.GET("/WasteTrader", () => {
                return Self.GET("/Waste2Value");
            });

            Handle.GET("/Waste2Value", () =>
            {
                var master = GetMasterPage();

                master.CurrentPage = Self.GET("/Waste2Value/partial/Home");

                return master;
            });

            Handle.GET("/Waste2Value/Salj", () =>
            {
                var master = GetMasterPage();

                master.CurrentPage = new SellPage();

                return master;
            });

            Handle.GET("/Waste2Value/Hitta", () =>
            {
                var master = GetMasterPage();

                master.CurrentPage = Self.GET("/Waste2Value/partial/Hitta");

                return master;
            });
        }

        private MasterPage GetMasterPage()
        {
            if (Session.Current == null)
            {
                Session.Current = new Session(SessionOptions.PatchVersioning);
            }

            MasterPage master = Session.Current.Data as MasterPage;

            if (master == null)
            {
                master = new MasterPage()
                {
                    Session = Session.Current,
                    NavigationBar = Self.GET("/Waste2Value/partial/navigationbar")
                };
            }

            return master;
        }
    }
}
