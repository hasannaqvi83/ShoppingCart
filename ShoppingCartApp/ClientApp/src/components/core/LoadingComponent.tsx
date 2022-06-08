import React, { useState } from 'react';
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';

interface Props {
  message?: string;
}

export default function LoadingComponent({ message = 'Loading...' }: Props) {
  const [smShow, setSmShow] = useState(false);
  const toggle = () => setSmShow(!smShow);
  const hide = () => setSmShow(false);
  return (
    <>
      <Modal
        isOpen
        toggle={toggle}
      >
        <ModalHeader toggle={toggle}>
          Modal title
        </ModalHeader>
        <ModalBody>
          Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
        </ModalBody>
        <ModalFooter>
          <Button
            color="primary"
            onClick={hide}
          >
            Do Something
          </Button>
          {' '}
          <Button onClick={hide}>
            Cancel
          </Button>
        </ModalFooter>
      </Modal>
    </>
  );
}