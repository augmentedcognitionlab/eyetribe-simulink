Simulink proxy for the Eye Tribe tracker
====

This is a little .NET application (runs on Windows, Mac and Linux) to redirect the Eye Tribe tracker data to a Simulik UDP receive block as an array of double values.

The proxy transmits 8 double values representing the following data:
 * Timestamp
 * Fixation (1 = true, 0 = false)
 * Smoothed X gaze coordinate
 * Smoothed Y gaze coordinate
 * Raw X gaze coordinate
 * Raw Y gaze coordinate
 * Left eye pupil size
 * Right eye pupil size

The data can be captured in Simulink using the UDP Receive block, setting the data type to double and the length of message to 8.
