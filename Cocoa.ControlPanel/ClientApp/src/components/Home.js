import 'rc-slider/assets/index.css';
import 'rc-tooltip/assets/bootstrap.css';
import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { PoseInput } from './PoseInput';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        this.handleOnChange = this.handleOnChange.bind(this);
        this.handleOnAfterChange = this.handleOnAfterChange.bind(this);

        this.state = {
            angles: [-1.0, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI, Math.PI]
        };

        this.getCurrentAngles();
    }    

    handleOnChange(id, value) {
        this.setState(state => {
            const angles = state.angles.map((item, j) => {
                if (j === id) {
                    return value;
                } else {
                    return item;
                }
            });

            return {
                angles,
            };
        });
    }

    handleOnAfterChange(id, value) {
        this.pose();
    }

    getCurrentAngles() {
        fetch('api/command/pose')         
            .then(response => response.json())
            .then(data => {
                for (var key in data.joints) {
                    var id = parseInt(data.joints[key].servoId);
                    var angle = parseFloat(data.joints[key].angle);
                    this.handleOnChange(id, angle);                  
                }
            });    
    }

    pose() {
        var pose = { poseid: 1, joints: [] };

        for (var key in this.state.angles) {
            pose.joints.push({ servoId: parseInt(key), angle: this.state.angles[key], duration: 1000 });
        }

        fetch('api/command/pose', {
            method: 'post',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(pose)
        })
    }

    stand() {
        fetch('api/command/stand', {
            method: 'post'
        })
    }

    lay() {
        fetch('api/command/lay', {
            method: 'post'
        })
    }

    render() {
        const angles = this.state.angles;
        return (
            <Container>        
                <Row>
                    <Col>
                        <PoseInput value={angles} onChange={this.handleOnChange} onAfterChange={this.handleOnAfterChange} />
                    </Col>
                </Row>
               <Row>
                    <Col><button className="btn btn-primary" onClick={this.stand}>Stand</button></Col>
                    <Col><button className="btn btn-primary" onClick={this.lay}>Lay Down</button></Col>
                    <Col><button className="btn btn-primary" onClick={() => this.getCurrentAngles()}>Capture</button></Col>
                </Row>
            </Container>
        );
    }
}
