﻿using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

namespace OlsonDigital.TestAutomation.Xunit
{

    /// <summary>
    /// Configuration for Remote Web Drivers
    /// </summary>
    public class RemoteWebDriverConfig
    {
        private bool _enabled;
        private string _user;
        private string _password;
        private Uri _url;

        private IDictionary<TargetBrowser, Dictionary<string, string>> _capabilities = new Dictionary<TargetBrowser, Dictionary<string, string>>();


        /// <summary>
        /// Hyrdates the a new Remote WebDriver Config from the provided section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public static RemoteWebDriverConfig Hydrate(IConfigurationSection section)
        {
            var config = new RemoteWebDriverConfig
            {
                _user = section["User"],
                _password = section["Password"],
                _url = section["Url"] != null ? new Uri(section["Url"]) : null
            };

            bool enabled = false;
            if ( bool.TryParse(section["Enabled"], out enabled) )
            {
                config._enabled = enabled;
            }

            var drivers = section.GetSection("Capabilities");
            foreach (var driver in drivers.GetChildren())
            {
                TargetBrowser targetBrowser;
                if (Enum.TryParse<TargetBrowser>(driver.Key, out targetBrowser))
                {
                    Dictionary<string, string> capabilities = new Dictionary<string, string>();
                    foreach (var setting in driver.GetChildren())
                    {
                        capabilities.Add(setting.Key, setting.Value);
                    }

                    config._capabilities.Add(targetBrowser, capabilities);
                }
            }

            return config;
        }

        /// <summary>
        /// Indicates if Remote Web Drivers are enabled.
        /// </summary>
        public bool Enabled => false;


        /// <summary>
        /// The username to use when authenticating with the remote service
        /// </summary>
        public string User => _user;


        /// <summary>
        /// The password to use when authenticating with the remote service
        /// </summary>
        public string Password => _password;


        /// <summary>
        /// The url of the remote service
        /// </summary>
        public Uri Url => _url;


        /// <summary>
        /// Browser specific capabilities to use when running remotely
        /// </summary>
        public IDictionary<TargetBrowser, Dictionary<string, string>> Capabiliities => _capabilities;
    }
}