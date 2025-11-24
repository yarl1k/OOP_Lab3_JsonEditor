using System;
using System.Collections.Generic;
using System.Linq;
using JsonEditor.Models;

namespace JsonEditor.Utilities
{
    public class EventManager
    {
        private List<SPF_Event> _events;
        private string _currentFilePath;
        private readonly JsonFileService _fileService;
        private SPF_Data _rootData;
        public bool IsFileLoaded => !string.IsNullOrEmpty(_currentFilePath);

        public EventManager()
        {
            _events = new List<SPF_Event>();
            _fileService = new JsonFileService();
            _rootData = new SPF_Data();
        }

        public async Task LoadFromFile(string path)
        {
            _currentFilePath = path;
            _rootData = await _fileService.LoadAsync(path);

            if (_rootData.Events == null) _rootData.Events = new List<SPF_Event>();

            _events = _rootData.Events;
        }

        public async Task SaveToFile()
        {
            if (string.IsNullOrEmpty(_currentFilePath)) return;

            _rootData.Events = _events;
            await _fileService.SaveAsync(_currentFilePath, _rootData);
        }

        public List<SPF_Event> GetAll() => _events;

        public void Add(SPF_Event newEvent)
        {
            int newId = 1;
            if (_events.Any())
            {
                newId = _events.Max(e => e.Event_id) + 1;
            }
            newEvent.Event_id = newId;
            _events.Add(newEvent);
        }
        public void Update(SPF_Event updatedEvent)
        {
            var existing = _events.FirstOrDefault(e => e.Event_id == updatedEvent.Event_id);
            if (existing != null)
            {
                existing.Event_name = updatedEvent.Event_name;
                existing.Department = updatedEvent.Department;
                existing.Event_date = updatedEvent.Event_date;
                existing.Event_time = updatedEvent.Event_time;
                existing.Event_location = updatedEvent.Event_location;
            }
        }

        public void Delete(int id)
        {
            var item = _events.FirstOrDefault(e => e.Event_id == id);

            if (item != null)
            {
                int deletedId = item.Event_id;
                int maxId = _events.Any() ? _events.Max(e => e.Event_id) : 0;
                _events.Remove(item);
                if (deletedId < maxId)
                {
                    for (int i = 0; i < _events.Count; i++)
                    {
                        _events[i].Event_id = i + 1;
                    }
                }
            }
        }

        public List<SPF_Event> Search(string text, string criterion)
        {
            return LINQ_Search.do_LINQ_search(_events, text, criterion);
        }
    }
}