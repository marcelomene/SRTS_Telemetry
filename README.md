# SRTS Telemetry

Software capable of capturing UDP telemetry packets from the game Gran Turismo 7 and displaying them graphically on a HUD, similar to those found in racing car steering wheels.

The program uses asynchronous tasks to listen for and capture new packets, adding them to a queue. A second asynchronous task consumes the packets from this queue, ensuring that no information is lost due to processing delays.

The processing logic was designed to be fully generic and easily expandable, allowing for the addition of support for other packet formats in the future. This way, new games and racing simulators can be easily integrated.

The telemetry packets contain essential data about the player's performance, such as speed, throttle and brake usage percentages, among others. In the future, the packets will be stored, and a lap-based graphical performance analysis system will be implemented.

![2025-01-31 16_48_04-SRTS GT7 Telemetry](https://github.com/user-attachments/assets/4e373b19-5d39-4ee0-ad7a-f95b0ef2c91d)

# Acknowledgments

User snimat from the GTPlanet forum, for consolidating information on telemetry (https://www.gtplanet.net/forum/threads/overview-of-gt7-telemetry-software.418011/)
User MacManley from GitHub, for their implementation of telemetry packet reading (https://github.com/MacManley/gt7-udp)
