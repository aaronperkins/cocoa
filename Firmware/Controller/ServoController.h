#ifndef _ServoController_h
#define _ServoController_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include <math.h>

struct ServoTrack
{
	float startAngle = -1;
	float targetAngle = -1;
	float currentAngle = -1;
	float nextAngle = -1;
	float speed;
	float distance;
	unsigned int duration;
	unsigned long startTime;
	unsigned long elapsedTime;
	bool moving = false;
};

class ServoController {
protected:
	unsigned char _servoCount;
	ServoTrack *_servoTrack;
	bool virtual setNextPosition(ServoTrack track[], unsigned char count);
	void virtual getCurrentPosition(ServoTrack track[], unsigned char start, unsigned char count);
	void virtual setTorque(bool enabled);
	void virtual setTorque(unsigned char id, bool enabled);
	void startMove(ServoTrack track[], unsigned char id, float position, unsigned int duration);
	void doFrame(ServoTrack track[], unsigned char count);

public:
	ServoController(unsigned char servoCount);
	void virtual begin();
	unsigned char servoCount();
	ServoTrack *servoTrack();
	void move(unsigned char id, float position, unsigned int duration);
	bool isMoving();
	void loop();
	void torque(bool enabled);
	void torque(unsigned char id, bool enabled);
};

#endif
