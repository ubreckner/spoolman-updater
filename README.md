# Spoolman Updater API

## Overview

The Spoolman Updater API provides endpoints to manage spool updates, including tracking filament usage and material details. This API is designed to work with Home Assistant and other automation systems.

To facilitate API development and testing, the Spoolman Updater API utilizes Swagger for interactive API documentation. You can access the Swagger UI at http://<your-server>:8088/swagger, which allows you to explore and test the available endpoints.

## Base URL

```
http://<your-server>:8088
```

## Endpoints

### 1. Update Spool

#### **Endpoint:**

```
POST /spool
```

#### **Description:**

Updates the spool details based on filament usage.

#### **Request Body:**

```json
{
  "name": "Bambu PLA Basic",
  "material": "PLA",
  "tagUid": "0000000000000000",
  "usedWeight": 10,
  "color": "#FFFFFFFF"
}
```

#### **Response:**

- **200 OK**: Successfully updated spool.
- **400 Bad Request**: Missing required fields.

---

## Environment Variables

The API requires the following environment variables to be set:

```
APPLICATION__HOMEASSISTANT__URL=http://homeassistant.local
APPLICATION__HOMEASSISTANT__TOKEN=your-token
APPLICATION__HOMEASSISTANT__TRAYSENSORPREFIX=sensor.x1c_tray_sensor_
APPLICATION__SPOOLMAN__URL=http://spoolman.local
```

## Running with Docker

### **Build the Docker image**

```
docker build -t spoolman-updater .
```

### **Run the container**

```
docker run -d -p 8088:8080 \
  -e APPLICATION__HOMEASSISTANT__URL=http://homeassistant.local \
  -e APPLICATION__HOMEASSISTANT__TOKEN=your-token \
  -e APPLICATION__SPOOLMAN__URL=http://spoolman.local \
  -e APPLICATION__HOMEASSISTANT__TRAYSENSORPREFIX=sensor.x1c_tray_sensor_ \
  --name spoolman-updater spoolman-updater
```

## Logging

The API logs requests and responses using the default ASP.NET logging system. You can configure logging levels in `appsettings.json`:

```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```

---

## Using with Home Assistant
The Spoolman Updater API can be integrated into Home Assistant automations to track filament usage automatically.

### **1. Define a REST Command in `configuration.yaml`**
Add the following to your `configuration.yaml` to create a REST command that updates the spool:

```yaml
rest_command:
  update_spool:
    url: "http://<your-server>:8088/Spools/spool"
    method: POST
    headers:
      Content-Type: "application/json"
    payload: >
      {
        "name": "{{ name }}",
        "material": "{{ material }}",
        "tag_uid": "{{ tag_uid }}",
        "used_weight": {{ used_weight }},
        "color": "{{ color }}"
      }
```

### **2. Create an Automation**
The following automation updates the spool when a print finishes or when the AMS tray switches:

```yaml
alias: Update Spool When Print Finishes or Tray Switches
trigger:
  - platform: state
    entity_id: sensor.x1c_print_status
    to: "idle"
  - platform: state
    entity_id: sensor.x1c_active_tray_index
condition:
  - condition: template
    value_template: "{{ states('sensor.x1c_active_tray_index') | int > 0 }}"
action:
  - variables:
      tray_number: "{{ trigger.to_state.state if trigger.entity_id == 'sensor.x1c_active_tray_index' else states('sensor.x1c_active_tray_index') }}"
      tray_sensor: "sensor.x1c_ams_tray_{{ tray_number }}"
      tray_weight: "{{ state_attr('sensor.x1c_print_weight', 'AMS 1 Tray ' ~ tray_number) | float(0) }}"
      tag_uid: "{{ state_attr(tray_sensor, 'tag_uid') }}"
      material: "{{ state_attr(tray_sensor, 'type') }}"
      name: "{{ state_attr(tray_sensor, 'name') }}"
      color: "{{ state_attr(tray_sensor, 'color') }}"
  - service: rest_command.update_spool
    data:
      name: "{{ name }}"
      material: "{{ material }}"
      tag_uid: "{{ tag_uid }}"
      used_weight: "{{ tray_weight }}"
      color: "{{ color }}"
```

This automation ensures that the filament usage is automatically updated in Spoolman when a print is completed or the AMS tray is changed.

---

## Contributing

Pull requests are welcome! Please follow the standard GitHub workflow:

1. Fork the repository
2. Create a feature branch
3. Submit a pull request

## License

MIT License. See `LICENSE` file for details.

