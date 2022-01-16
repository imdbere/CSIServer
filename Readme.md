## CSI Server
This tool consumes CSI data from the Atheros CSI tool device and exposes it via a REST Api.
It uses [CSI Lib](https://github.com/imdbere/CSILib) to parse the incoming data.

The tool exposes two REST routes:
- **/csi/latest**
This returns the latest received CSI data for each subcarrier and RX/TX pair. It is useful to create visualizations of the current state.
- **/csi?tx=0&rx=0&subcarrier=0&numSamples=5**
This returns the last n samples (configurable by numSamples) for a given combination of RX/TX antenna and subcarrier. It is useful for creating time-series visualizations (such as displaying the changes of a specific value over the last seconds)