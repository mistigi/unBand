﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using unBand.Cloud;

namespace unBand.CloudHelpers
{
    public class BandEventViewModel :INotifyPropertyChanged
    {
        public BandEventBase Event { get; private set; }

        private BandCloudClient _cloud;
        private bool _loaded = false;

        public bool Loaded
        {
            get { return _loaded; }
            set
            {
                if (_loaded != value)
                {
                    _loaded = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public BandEventViewModel(BandCloudClient cloud, BandEventBase cloudEvent)
        {
            _cloud = cloud;

            Event = cloudEvent;
            Event.EventDataDownloaded += Event_EventDataDownloaded;
        }

        public async Task LoadFull()
        {
            Event.InitFullEventData(await _cloud.GetFullEventData(Event.EventID, Event.Expanders));
        }

        ~BandEventViewModel()
        {
            Event.EventDataDownloaded -= Event_EventDataDownloaded;
        }

        void Event_EventDataDownloaded(BandEventBase bandEvent)
        {
            Loaded = true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }));
            }
        }

        #endregion

    }
}
