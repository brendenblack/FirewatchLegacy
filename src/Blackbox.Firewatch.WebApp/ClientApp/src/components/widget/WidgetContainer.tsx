import React, { Component } from 'react';
import { StyleSheet, css } from 'aphrodite';
import { Colors, Button, Icon } from "@blueprintjs/core";
import { IconName } from "@blueprintjs/icons";

const styles = StyleSheet.create({
    root: {
        borderStyle: 'solid',
        borderWidth: 1,
        borderColor: Colors.DARK_GRAY2,
        borderRadius: 4,
        boxShadow: '1px 2px ' + Colors.DARK_GRAY1,
    },
    header: {
        backgroundColor: Colors.DARK_GRAY2,
        width: '100%',
        display: 'flex',
        padding: '.2em'
    },
    headerTitle: {
        flexGrow: 1,
    }
});

interface WidgetContainerProps {
    title?: string;
    icon?: IconName;
    onClose?(): void;
    onExpand?(): void;
}


export class WidgetContainer extends Component<WidgetContainerProps> {
    constructor(props: WidgetContainerProps) {
        super(props);
    }

    handleExpandClick() {
        if (this.props.onExpand) {
        this.props.onExpand();
        } else {
            console.warn('No handler has been registered to handle expanding this widget.');
        }
    }

    handleExitClick() {
        if (this.props.onClose) {
        this.props.onClose();
        } else {
            console.warn('No handler has been registered for handling an exit click for this widget.');
        }
    }

    render() {
        const icon = this.props.icon ? <Icon icon={this.props.icon} /> : '';
        const title = this.props.title ?? 'Widget';

        const onExpand: JSX.Element = (this.props.onExpand) 
            ? <Button onClick={this.handleExpandClick} minimal icon="maximize" />
            : <span />

        const onClose: JSX.Element = (this.props.onClose) 
            ? <Button onClick={this.handleExpandClick} minimal icon="cross" />
            : <span />

            // maximize
            // close


        return (
            <div className={css(styles.root)}>
                <div className={css(styles.header)}>
                    <span className={css(styles.headerTitle) + " bp3-text-large"}>{icon}{title}</span>
                    {onExpand}
                    {onClose}
                </div>
                {this.props.children}
            </div>
        );
    }
}
