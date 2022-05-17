﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OWSData.SQL
{
    public static class MSSQLQueries
    {
		public static readonly string AddOrUpdateWorldServerSQL = @"MERGE WorldServers AS tbl
				USING (SELECT @CustomerGUID AS CustomerGUID, 
					@ServerIP AS ServerIP, 
					@MaxNumberOfInstances AS MaxNumberOfInstances,
					8081 as Port,
					0 as ServerStatus,
					@InternalServerIP as InternalServerIP,
					@StartingMapInstancePort as StartingMapInstancePort,
					@ZoneServerGUID as ZoneServerGUID) AS row
					ON tbl.CustomerGUID = Row.CustomerGUID AND tbl.ZoneServerGUID = row.ZoneServerGUID
				WHEN NOT MATCHED THEN
					INSERT(CustomerGUID, ServerIP, MaxNumberOfInstances, Port, ServerStatus, InternalServerIP, StartingMapInstancePort, ZoneServerGUID)
					VALUES(row.CustomerGUID, row.ServerIP, row.MaxNumberOfInstances, row.Port, row.ServerStatus, row.InternalServerIP, row.StartingMapInstancePort, row.ZoneServerGUID)
				WHEN MATCHED THEN
					UPDATE SET
					tbl.MaxNumberOfInstances = row.MaxNumberOfInstances,
					tbl.Port = row.Port,
					tbl.ServerStatus = row.ServerStatus,
					tbl.InternalServerIP = row.InternalServerIP,
					tbl.StartingMapInstancePort = row.StartingMapInstancePort,
					tbl.ZoneServerGUID = row.ZoneServerGUID;";

        public static readonly string GetUserSessionSQL = @"SELECT US.CustomerGUID, US.UserGUID, US.UserSessionGUID, US.LoginDate, US.SelectedCharacterName,
	            U.Email, U.FirstName, U.LastName, U.CreateDate, U.LastAccess, U.Role,
	            C.CharacterID, C.CharName, C.X, C.Y, C.Z, C.RX, C.RY, C.RZ, C.MapName as ZoneName
	            FROM UserSessions US
	            INNER JOIN Users U
		            ON U.UserGUID=US.UserGUID
	            LEFT JOIN Characters C
		            ON C.CustomerGUID=US.CustomerGUID
		            AND C.CharName=US.SelectedCharacterName
	            WHERE US.CustomerGUID=@CustomerGUID
	            AND US.UserSessionGUID=@UserSessionGUID";

		public static readonly string GetUserSessionOnlySQL = @"SELECT US.CustomerGUID, US.UserGUID, US.UserSessionGUID, US.LoginDate, US.SelectedCharacterName
	            FROM UserSessions US
	            WHERE US.CustomerGUID=@CustomerGUID
	            AND US.UserSessionGUID=@UserSessionGUID";

		public static readonly string GetUserSQL = @"SELECT U.Email, U.FirstName, U.LastName, U.CreateDate, U.LastAccess, U.Role
	            FROM Users U
	            WHERE U.CustomerGUID=@CustomerGUID
	            AND U.UserGUID=@UserGUID";

		public static readonly string GetUserFromEmailSQL = @"SELECT U.Email, U.FirstName, U.LastName, U.CreateDate, U.LastAccess, U.Role
	            FROM Users U
	            WHERE U.CustomerGUID=@CustomerGUID
	            AND U.Email=@Email";

		public static readonly string GetCharacterByNameSQL = @"SELECT C.CharacterID, C.CharName, C.X, C.Y, C.Z, C.RX, C.RY, C.RZ, C.MapName as ZoneName
	            FROM Characters C
	            WHERE C.CustomerGUID=@CustomerGUID
	            AND C.CharName=@CharacterName";

		public static readonly string GetWorldServerSQL = @"SELECT WorldServerID
				FROM WorldServers 
				WHERE CustomerGUID=@CustomerGUID 
				AND ZoneServerGUID=@ZoneServerGUID";

		public static readonly string UpdateWorldServerSQL = @"UPDATE WorldServers
				SET ActiveStartTime=GETDATE(),
				ServerStatus=1
				WHERE CustomerGUID=@CustomerGUID 
				AND WorldServerID=@WorldServerID";
	}
}
