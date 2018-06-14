// #StartHeader# ==============================================================
//
// This file is a part of the SMARTDataBees open source project.
// 
// Copyright (C) 2007 by
//        G.E.M. Team Solutions GbR
//        CAD-Development
//
// SMARTDataBees is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SMARTDataBees is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SMARTDataBees.  If not, see <http://www.gnu.org/licenses/>.
//
// #EndHeader# ================================================================

using System;
using System.Windows.Forms;
using Carbon.Plugins;
using SDBees.DB;
using SDBees.Main.Window;

namespace SDBees.Plugs.TemplateBase
{
    public abstract class TemplateBase : Plugin
    {
        private SDBeesDBConnection _DBManager;
        private MainWindowApplication _MainWindow;
        private PluginContext m_context;

        public PluginContext MyPluginContext
        {
            get { return m_context; }
            set { m_context = value; }
        }

        public void ReStart(PluginContext context)
        {
            Start(context, null);
        }

        /// <summary>
        /// Your have to call this, to ensure that the DBManager and Mainwindow are set up correctly
        /// </summary>
        /// <param name="context"></param>
        /// <param name="e"></param>
        public void StartMe(PluginContext context, PluginDescriptorEventArgs e)
        {
            try
            {
                m_context = context;

                Console.WriteLine("TemplateBase starts\n");

                //Das Databaseplugin besorgen
                if (m_context.PluginDescriptors.Contains(new PluginDescriptor(typeof(SDBeesDBConnection))))
                {
                    _DBManager = (SDBeesDBConnection)m_context.PluginDescriptors[typeof(SDBeesDBConnection)].PluginInstance;
                    _DBManager.AddDatabaseChangedHandler(OnDatabaseChanged);
                    _DBManager.AddUpdateHandler(OnUpdate);
                }
                else
                {
                    MessageBox.Show("Es konnte kein Datenbankmanager gefunden werden!", ToString());
                    _DBManager = null;
                }

                //Das MainWindowplugin besorgen
                if (m_context.PluginDescriptors.Contains(new PluginDescriptor(typeof(MainWindowApplication))))
                {
                    _MainWindow = (MainWindowApplication)m_context.PluginDescriptors[typeof(MainWindowApplication)].PluginInstance;
                }
                else
                {
                    MessageBox.Show("Es konnte kein Hauptfenster gefunden werden!", ToString());
                    _MainWindow = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //throw;
            }
        }

        public SDBeesDBConnection MyDBManager
        {
            get { return _DBManager; }
        }

        public MainWindowApplication MyMainWindow
        {
            get { return _MainWindow; }
        }

        // Must be overridden by derived class!
        protected abstract void OnDatabaseChanged(object sender, EventArgs e);

        // Can be overridden by derived class!
        protected virtual void OnUpdate(object sender, EventArgs e) { /* empty */ }
    }
}
