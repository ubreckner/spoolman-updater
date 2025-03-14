# Spoolman Updater API

## Overview

The Spoolman Updater API provides endpoints to manage spool updates, including tracking filament usage and material details. This API is designed to work with Home Assistant and other automation systems.

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

### 2. Get Spool by Brand and Color

#### **Endpoint:**

```
GET /spools/brand/{brand}/color/{color}
```

#### **Description:**

Fetches a spool based on brand and color.

#### **Response:**

```json
{
  "id": 1,
  "name": "Bambu PLA Basic",
  "brand": "Bambu",
  "material": "PLA",
  "color": "#FFFFFFFF",
  "weight": 750
}
```

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
docker run -d -p 8088:8088 \
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

## Contributing

Pull requests are welcome! Please follow the standard GitHub workflow:

1. Fork the repository
2. Create a feature branch
3. Submit a pull request

## License

MIT License. See `LICENSE` file for details.

