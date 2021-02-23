﻿using DocuSign.Rooms.Api;
using DocuSign.Rooms.Client;
using DocuSign.Rooms.Model;
using System.Collections.Generic;
using System.Linq;

namespace DocuSign.Rooms.Examples
{
    public class AddingFormToRoom
    {
        /// <summary>
        /// Gets the list of rooms
        /// </summary>
        /// <param name="basePath">BasePath for API calls (URI)</param>
        /// <param name="accessToken">Access Token for API call (OAuth)</param>
        /// <param name="accountId">The DocuSign Account ID (GUID or short version) for which the APIs call would be made</param>
        /// <returns>The list of rooms</returns>
        public static (FormSummaryList forms, RoomSummaryList rooms) GetFormsAndRooms(string basePath, string accessToken, string accountId)
        {
            // Construct your API headers
            var apiClient = new ApiClient(basePath);
            apiClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accessToken);
            var roomsApi = new RoomsApi(apiClient);
            var formLibrariesApi = new FormLibrariesApi(apiClient);

            // Get Forms Libraries
            FormLibrarySummaryList formLibraries = formLibrariesApi.GetFormLibraries(accountId);

            // Get Forms 
            FormSummaryList forms = new FormSummaryList(new List<FormSummary>());
            if (formLibraries.FormsLibrarySummaries.Any())
            {
                forms = formLibrariesApi.GetFormLibraryForms(
                    accountId,
                    formLibraries.FormsLibrarySummaries.First().FormsLibraryId);
            }

            // Get Rooms 
            RoomSummaryList rooms = roomsApi.GetRooms(accountId);

            // Call the Rooms API to create a room
            return (forms, rooms);
        }

        
    }
}
