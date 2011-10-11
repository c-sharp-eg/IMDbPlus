﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MediaPortal.Profile;
using MediaPortal.Configuration;

namespace IMDb
{
    public static class PluginSettings
    {
        public static string OptionsFile = Path.Combine(Config.GetFolder(Config.Dir.Config), @"IMDb+\Options IMDb+ Scraper.xml");

        #region Settings

        public static bool OriginalTitle { get; set; }
        public static bool ForeignTitle { get; set; }
        public static bool ForeignFirst { get; set; }
        public static bool SpecialEdition { get; set; }
        public static bool RenameTitles { get; set; }
        public static bool SingleScore { get; set; }
        public static bool ImdbScore { get; set; }
        public static bool ImdbMetaScore { get; set; }
        public static bool RottenMeter { get; set; }
        public static bool RottenAverage { get; set; }
        public static bool RottenTopCritics { get; set; }
        public static bool MinImdbVotes { get; set; }
        public static bool LongSummary { get; set; }
        public static bool UkRating { get; set; }
        public static bool OneWriterDirector { get; set; }
        public static string SecondaryDetails { get; set; }
        public static bool SecondarySummary { get; set; }
        public static bool RefreshAllFields { get; set; }
        public static string CountryFilter { get; set; }
        public static string LanguageFilter { get; set; }
        public static int SyncInterval { get; set; }
        public static bool SyncOnStartup { get; set; }
        public static string SyncLastDateTime { get; set; }

        #endregion

        #region Constants

        private const string cOriginalTitle = "global_options_original_title";
        private const string cForeignTitle = "global_options_foreign_title";
        private const string cForeignFirst = "global_options_foreign_first";
        private const string cSpecialEdition = "global_options_special_edition";
        private const string cRenameTitles = "global_options_rename_titles";
        private const string cSingleScore = "global_options_single_score";
        private const string cImdbScore = "global_options_imdb_score";
        private const string cImdbMetaScore = "global_options_imdb_metascore";
        private const string cRottenMeter = "global_options_rotten_meter";
        private const string cRottenAverage = "global_options_rotten_average";
        private const string cRottenTopCritics = "global_options_rotten_top_critics";
        private const string cMinImdbVotes = "global_options_min_imdb_votes";
        private const string cLongSummary = "global_options_long_summary";
        private const string cUkRating = "global_options_uk_rating";
        private const string cOneWriterDirector = "global_options_one_writer_director";
        private const string cSecondaryDetails = "global_options_secondary_details";
        private const string cSecondarySummary = "global_options_secondary_summary";
        private const string cRefreshAllFields = "global_options_refresh_all_fields";
        private const string cCountryFilter = "global_options_country_filter";
        private const string cLanguageFilter = "global_options_language_filter";
        private const string cSyncInterval = "global_options_sync_interval";
        private const string cSyncOnStartup = "global_options_sync_on_startup";
        private const string cSyncLastDateTime = "global_options_sync_last_datetime";
        
        #endregion

        public static string Version
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Loads the Settings
        /// </summary>
        public static void LoadSettings()
        {
            Logger.Info("Loading IMDb+ options from file.");
            XmlReader xmlReader = new XmlReader();
            if (!xmlReader.Load(OptionsFile))
            {
                Logger.Error("Error opening IMDb+ Options file, will restore defaults.");                
            }
            
            OriginalTitle = xmlReader.GetOptionValueAsBool(cOriginalTitle, false);
            ForeignTitle = xmlReader.GetOptionValueAsBool(cForeignTitle, false);
            ForeignFirst = xmlReader.GetOptionValueAsBool(cForeignFirst, false);
            SpecialEdition = xmlReader.GetOptionValueAsBool(cSpecialEdition, true);
            RenameTitles = xmlReader.GetOptionValueAsBool(cRenameTitles, true);
            SingleScore = xmlReader.GetOptionValueAsBool(cSingleScore, false);
            ImdbScore = xmlReader.GetOptionValueAsBool(cImdbScore, false);
            ImdbMetaScore = xmlReader.GetOptionValueAsBool(cImdbMetaScore, false);
            RottenMeter = xmlReader.GetOptionValueAsBool(cRottenMeter, false);
            RottenAverage = xmlReader.GetOptionValueAsBool(cRottenAverage, false);
            RottenTopCritics = xmlReader.GetOptionValueAsBool(cRottenTopCritics, false);
            MinImdbVotes = xmlReader.GetOptionValueAsBool(cMinImdbVotes, false);
            LongSummary = xmlReader.GetOptionValueAsBool(cLongSummary, false);
            UkRating = xmlReader.GetOptionValueAsBool(cUkRating, false);
            OneWriterDirector = xmlReader.GetOptionValueAsBool(cOneWriterDirector, false);
            SecondaryDetails = xmlReader.GetOptionValueAsString(cSecondaryDetails, "01");
            SecondarySummary = xmlReader.GetOptionValueAsBool(cSecondarySummary, false);
            RefreshAllFields = xmlReader.GetOptionValueAsBool(cRefreshAllFields, false);
            CountryFilter = xmlReader.GetOptionValueAsString(cCountryFilter, "us|ca|gb|ie|au|nz");
            LanguageFilter = xmlReader.GetOptionValueAsString(cLanguageFilter, "en");
            SyncInterval = xmlReader.GetOptionValueAsInt(cSyncInterval, 24);
            SyncOnStartup = xmlReader.GetOptionValueAsBool(cSyncOnStartup, true);
            SyncLastDateTime = xmlReader.GetOptionValueAsString(cSyncLastDateTime, DateTime.MinValue.ToString());

            // save settings, might be some new settings added
            SaveSettings();
        }

        /// <summary>
        /// Saves the Settings
        /// </summary>
        public static void SaveSettings()
        {
            Logger.Info("Saving IMDb+ options to file.");

            XmlWriter xmlWriter = new XmlWriter();
            if (!xmlWriter.Load(OptionsFile))
            {
                if (File.Exists(OptionsFile))
                {
                    try
                    {
                        File.Delete(OptionsFile);
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Error deleting file: '{0}'", OptionsFile);
                        Logger.Error("Exception: {0}", e.Message);
                        return;
                    }
                }

                // create it
                Logger.Info("Creating new IMDb+ options file.");
                xmlWriter.CreateXmlConfigFile(OptionsFile);
            }

            xmlWriter.SetOptionsEntry(cOriginalTitle, "01", OriginalTitle.ToString());
            xmlWriter.SetOptionsEntry(cForeignTitle, "02", ForeignTitle.ToString());
            xmlWriter.SetOptionsEntry(cForeignFirst, "03", ForeignFirst.ToString());
            xmlWriter.SetOptionsEntry(cUkRating, "04", UkRating.ToString());
            xmlWriter.SetOptionsEntry(cImdbScore, "05", ImdbScore.ToString());
            xmlWriter.SetOptionsEntry(cImdbMetaScore, "06", ImdbMetaScore.ToString());
            xmlWriter.SetOptionsEntry(cLongSummary, "07", LongSummary.ToString());
            xmlWriter.SetOptionsEntry(cRottenMeter, "08", RottenMeter.ToString());
            xmlWriter.SetOptionsEntry(cRottenAverage, "09", RottenAverage.ToString());
            xmlWriter.SetOptionsEntry(cRottenTopCritics, "10", RottenTopCritics.ToString());
            xmlWriter.SetOptionsEntry(cSpecialEdition, "11", SpecialEdition.ToString());
            xmlWriter.SetOptionsEntry(cRenameTitles, "12", RenameTitles.ToString());
            xmlWriter.SetOptionsEntry(cSingleScore, "13", SingleScore.ToString());
            xmlWriter.SetOptionsEntry(cMinImdbVotes, "14", MinImdbVotes.ToString());
            xmlWriter.SetOptionsEntry(cRefreshAllFields, "15", RefreshAllFields.ToString());
            xmlWriter.SetOptionsEntry(cOneWriterDirector, "16", OneWriterDirector.ToString());

            xmlWriter.SetOptionsEntry(cSecondarySummary, "96", SecondarySummary.ToString());
            xmlWriter.SetOptionsEntry(cSecondaryDetails, "97", SecondaryDetails);
            xmlWriter.SetOptionsEntry(cCountryFilter, "98", CountryFilter);
            xmlWriter.SetOptionsEntry(cLanguageFilter, "99", LanguageFilter);

            xmlWriter.SetOptionsEntry(cSyncInterval, "200", SyncInterval.ToString());
            xmlWriter.SetOptionsEntry(cSyncOnStartup, "201", SyncOnStartup.ToString());
            xmlWriter.SetOptionsEntry(cSyncLastDateTime, "202", SyncLastDateTime);

            // save file
            xmlWriter.Save(OptionsFile);
        }
    }
}