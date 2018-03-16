﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SynAppsLuis
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="synapps_iot")]
	public partial class SynAppsSyncStatusesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertSynAppsSyncStatuse(SynAppsSyncStatuse instance);
    partial void UpdateSynAppsSyncStatuse(SynAppsSyncStatuse instance);
    partial void DeleteSynAppsSyncStatuse(SynAppsSyncStatuse instance);
    #endregion
		
		public SynAppsSyncStatusesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SynAppsSyncStatusesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SynAppsSyncStatusesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SynAppsSyncStatusesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<SynAppsSyncStatuse> SynAppsSyncStatuses
		{
			get
			{
				return this.GetTable<SynAppsSyncStatuse>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="RBApp.SynAppsSyncStatuses")]
	public partial class SynAppsSyncStatuse : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _SynAppsDeviceId;
		
		private string _Status;
		
		private System.DateTime _LastTrainingDateTime;
		
		private System.DateTime _CreatedAt;
		
		private System.DateTime _UpdatedAt;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnSynAppsDeviceIdChanging(string value);
    partial void OnSynAppsDeviceIdChanged();
    partial void OnStatusChanging(string value);
    partial void OnStatusChanged();
    partial void OnLastTrainingDateTimeChanging(System.DateTime value);
    partial void OnLastTrainingDateTimeChanged();
    partial void OnCreatedAtChanging(System.DateTime value);
    partial void OnCreatedAtChanged();
    partial void OnUpdatedAtChanging(System.DateTime value);
    partial void OnUpdatedAtChanged();
    #endregion
		
		public SynAppsSyncStatuse()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SynAppsDeviceId", DbType="NVarChar(255)")]
		public string SynAppsDeviceId
		{
			get
			{
				return this._SynAppsDeviceId;
			}
			set
			{
				if ((this._SynAppsDeviceId != value))
				{
					this.OnSynAppsDeviceIdChanging(value);
					this.SendPropertyChanging();
					this._SynAppsDeviceId = value;
					this.SendPropertyChanged("SynAppsDeviceId");
					this.OnSynAppsDeviceIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
		public string Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LastTrainingDateTime", DbType="DateTime NOT NULL")]
		public System.DateTime LastTrainingDateTime
		{
			get
			{
				return this._LastTrainingDateTime;
			}
			set
			{
				if ((this._LastTrainingDateTime != value))
				{
					this.OnLastTrainingDateTimeChanging(value);
					this.SendPropertyChanging();
					this._LastTrainingDateTime = value;
					this.SendPropertyChanged("LastTrainingDateTime");
					this.OnLastTrainingDateTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedAt", DbType="DateTime NOT NULL")]
		public System.DateTime CreatedAt
		{
			get
			{
				return this._CreatedAt;
			}
			set
			{
				if ((this._CreatedAt != value))
				{
					this.OnCreatedAtChanging(value);
					this.SendPropertyChanging();
					this._CreatedAt = value;
					this.SendPropertyChanged("CreatedAt");
					this.OnCreatedAtChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UpdatedAt", DbType="DateTime NOT NULL")]
		public System.DateTime UpdatedAt
		{
			get
			{
				return this._UpdatedAt;
			}
			set
			{
				if ((this._UpdatedAt != value))
				{
					this.OnUpdatedAtChanging(value);
					this.SendPropertyChanging();
					this._UpdatedAt = value;
					this.SendPropertyChanged("UpdatedAt");
					this.OnUpdatedAtChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591