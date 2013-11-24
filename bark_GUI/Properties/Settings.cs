using System.Collections;
using System.IO;

namespace bark_GUI.Properties {
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    internal sealed partial class Settings {
        
        public Settings() {
            if(MenuRecentFiles == null) MenuRecentFiles = new ArrayList();
            if (PathMainDirectory == null) PathMainDirectory = @Directory.GetCurrentDirectory();
            if (PathSamples == null) PathSamples = PathMainDirectory + @"\Samples";
            if (PathMaterials == null) PathMaterials = PathMainDirectory + @"\Materials";
            if (PathErrorLog == null) PathErrorLog = PathMainDirectory + @"\";
            if (PathBarkExe == null) PathBarkExe = @"C:\Program Files (x86)\bark\0.5\bin";
            PathCurrentFile = @"";
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Add code to handle the SettingChangingEvent event here.
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Add code to handle the SettingsSaving event here.
        }
    }
}
