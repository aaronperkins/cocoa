#ifndef _ServoControllerAx12A_h
#define _ServoControllerAx12A_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include "ServoController.h"
#include "AX12A.h"

const float RAW_TO_RAD = 0.00613592315; // 2pi / 1024
const float RAD_TO_RAW = 162.974661726; // 1024 / 2pi
const int MAX_POS = 1023;

class ServoControllerAx12A : public ServoController {
protected:
	HardwareSerial &_serial;
	long _baudRate;
	unsigned char _directionPin;
	int *_ax12Position;
	int radiansToRaw(float radians);
	float rawToRadians(int raw);
	bool setNextPosition(ServoTrack track[], unsigned char count);
	void getCurrentPosition(ServoTrack track[], unsigned char start, unsigned char count);
	void setTorque(bool enabled);
	void setTorque(unsigned char id, bool enabled);

public:
	ServoControllerAx12A(unsigned char servoCount, long baudRate, unsigned char directionPin, HardwareSerial &serial);
	void begin();
};

#endif