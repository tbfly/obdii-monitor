﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ObdiiMonitor
{
    public class Controller
    {
        private MainWindow mainWindow;

        public MainWindow MainWindow
        {
            get { return mainWindow; }
            set { mainWindow = value; }
        }

        private TCWindow tcWindow = new TCWindow();

        public TCWindow TcWindow
        {
            get { return tcWindow; }
        }

        private SensorController sensorController = new SensorController();

        public SensorController SensorController
        {
            get { return sensorController; }
        }

        private LoadController loadController = new LoadController();

        internal LoadController LoadController
        {
            get { return loadController; }
        }

        private SaveController saveController = new SaveController();

        internal SaveController SaveController
        {
            get { return saveController; }
        }

        private LiveDataController liveDataController = new LiveDataController();

        internal LiveDataController LiveDataController
        {
            get { return liveDataController; }
        }

        private Serial serial = new Serial();

        public Serial Serial
        {
            get { return serial; }
        }
        
        private bool us = false;

        public bool US
        {
            get { return us; }
            set { us = value; }
        }

        private SensorData sensorData = new SensorData();

        public SensorData SensorData
        {
            get { return sensorData; }
        }

        private AccelerometerConverter accelerometerConverver = new AccelerometerConverter();

        internal AccelerometerConverter AccelerometerConverver
        {
            get { return accelerometerConverver; }
        }

        private TimeOfDayConverter timeOfDayConverter = new TimeOfDayConverter();

        internal TimeOfDayConverter TimeOfDayConverter
        {
            get { return timeOfDayConverter; }
        }

        private GPS gps = new GPS();

        internal GPS Gps
        {
            get { return gps; }
        }

        private byte[] config = new byte[84];

        public byte[] Config
        {
            get { return config; }
            set { config = value; }
        }

        public Controller()
        {
            sensorController.Controller = this;
            loadController.Controller = this;
            saveController.Controller = this;
            serial.Controller = this;
            sensorData.Controller = this;
            accelerometerConverver.Controller = this;
            gps.Controller = this;
            tcWindow.Controller = this;
            liveDataController.Controller = this;
            timeOfDayConverter.Controller = this;
        }

        public void reset()
        {
            // reset the calibration reading for the accelerometer as this is a new session.
            accelerometerConverver.resetCalibrationReading();

            // clear the pollResponses ArrayList member in SensorData to begin loading in new data
            sensorData.clearPollResponses();

            // Clear the GraphQueue, not sure if the GraphQueue will stick around.
            mainWindow.GraphQueue.Clear();

            // clear the gps linked list
            gps.GpsList.Clear();

            // cancel all the threads
            cancelAllThreads();
        }

        public void cancelAllThreads()
        {
            if (MainWindow.UpdateGraphPlots != null)
                MainWindow.UpdateGraphPlots.Abort();

            if (sensorController.receiving != null)
                sensorController.receiving.Abort();
        }
    }
 }