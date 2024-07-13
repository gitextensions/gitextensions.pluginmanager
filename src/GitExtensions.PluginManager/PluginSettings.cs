using GitExtensions.Extensibility.Settings;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GitExtensions.PluginManager
{
    internal class PluginSettings : IEnumerable<ISetting>
    {
        /// <summary>
        /// Gets a property holding if asking to close git extensions is required.
        /// </summary>
        public static BoolSetting CloseInstancesProperty { get; } = new BoolSetting("CloseInstances", "Close all instances of Git Extensions before starting Plugin Manager", false);

        private readonly SettingsSource source;

        /// <summary>
        /// Gets current value of <see cref="CloseInstancesProperty"/>.
        /// </summary>
        public bool CloseInstances => source.GetBool(CloseInstancesProperty.Name, CloseInstancesProperty.DefaultValue);

        public PluginSettings(SettingsSource source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        #region IEnumerable<ISetting>

        private static readonly List<ISetting> properties;

        public static bool HasProperties => properties.Count > 0;

        static PluginSettings()
        {
            properties = new List<ISetting>(1)
            {
                CloseInstancesProperty
            };
        }

        public IEnumerator<ISetting> GetEnumerator()
            => properties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion
    }
}
