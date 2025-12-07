# ChargerAstronomyShared

This repository acts as an intermediate repository between the "frontend" and "backend" of Stargazer. It contains structures, interfaces, and sealed objects that are used in both repositories to organize development, establish common definitions, and seperate the engine from its Unity implementation.


## Contracts

This directory contains models for "contractual" data, essenentially objects that are instantiated/utilized on specific conditions (typically program startup).


## Domain

This directory contains a majority of the classes that are frequently passed from front to back end. Each directory contains classes related to its purpose.

- **Equatorial** contains definitions related to equatorial objects. This includes BodyTypes, which are used for planets.
- **Geometry** contains tile geometry, which contains useful methodes for obtaining tile information for spatial indexing calculations.
- **Heat** contains definitions related to the heat indexing system, including the HeatConfig and HeatMap classes.
- **Horizontal** contains definitions related to horizontal objects.
- **Index** contains definitions and classes for the spatial indexing system. This includes the spatial index itself as well as different tile indexes (icospheric, UV spheric, cubic).
- **Prediction** contains the tile selector method used for the heat index logic. Currently it only contains one method of selection, which relies on the users' camera direction and FOV.
