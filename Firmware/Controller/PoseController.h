#ifndef _PoseControllerAx12A_h
#define _PoseControllerAx12A_h

#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include "ServoController.h"
#include <RingBufCPP.h>

struct Joint
{
	float angle = -1;
	unsigned int duration;
};

struct Pose
{
	unsigned int id = 0;
	Joint *joint;
};

class PoseController {
private:
	ServoController &_servoController;
	RingBufCPP<struct Pose, 16> _poseBuffer;
	bool _isMoving = false;
	int _currentPoseId = 0;
	void(*_poseCompleteCallback)(int) = NULL;

public:
	PoseController(ServoController &servoController);
	void setPoseCompleteCallback(void (*poseCompleteCallback)(int));
	void queuePose(Pose &pose);
	void loop();
};

#endif
