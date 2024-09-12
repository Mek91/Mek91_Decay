# Mek91_Decay
Rocket.Unturned is an Unturned Plugin made with Unturned, its purpose is to reduce the life of all kinds of items that players can place over time and decay them.

👇**MY DISCORD**👇  
Please indicate any shortcomings or ideas.  
[Join the Discord](https://discord.gg/Fc3UjkUK5T)

👇**SETUP**👇
First of all, [‘Download From Here’](https://github.com/Mek91/Mek91_Decay/releases/download/mek91_decay.v1.0.0.0/Mek91_Decay.Version1.0.0.0.rar) then extract the .rar file you downloaded directly into the ‘Rocket’ in your server file and that's it.

👇**EXAMPLE PLUGIN CONFIG**👇
```
<?xml version="1.0" encoding="utf-8"?>
<DecayConfig xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <!-- 
    HealCount: 
    This value should always be higher than "DecayAmount" and is used to increase the health of placeable items.
    Ensures that the repair amount is sufficient to counteract decay.
  -->
  <HealCount>6</HealCount> 
  
  <!-- 
    DecayIntervalMinute: 
    Defines the interval in minutes at which decay damage is applied and repaired, resulting in ongoing decay effects. 
    For example, if set to 20 minutes, the system will continuously apply decay damage and repair within this interval.
  -->
  <DecayIntervalMinute>20</DecayIntervalMinute>
  
  <!-- 
    DecayAmount: 
    This value should always be lower than "HealCount" because if it's higher, the healing effect might be insufficient,
    leading to more damage than healing.
  -->
  <DecayAmount>5</DecayAmount>
  
  <!-- 
    Whitelist_NoDecay: 
    This section represents items that should not be affected by decay. List items here to ensure they are not damaged by decay,
    Make sure the items listed are placeable in the game environment.
  -->
  <Whitelist_NoDecay>
    <unsignedLong>1241</unsignedLong>
    <!-- <unsignedLong>0</unsignedLong> -->
    <!-- <unsignedLong>0</unsignedLong> -->
    <!-- <unsignedLong>0</unsignedLong> -->
  </Whitelist_NoDecay>
</DecayConfig>

```
